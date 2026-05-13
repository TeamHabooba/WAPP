// CourseView.razor.cs
using Microsoft.AspNetCore.Components;
using PwnLearn.Models;
using PwnLearn.Services;

namespace PwnLearn.Pages.Member;

public partial class CourseView : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private IQuizService QuizService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====Parameters
    [Parameter] public int CourseId { get; set; }

    // =====State
    private Course? _course;
    private Module? _activeModule;
    private int _activeModuleId;
    private bool _isEnrolled;
    private bool _isLoading = true;

    // Module feedback
    private string? _moduleMsg;
    private string _moduleMsgType = "success";

    // Enrol feedback
    private string? _enrollMsg;
    private string _enrollMsgType = "success";

    // Quiz state
    private List<QuizQuestion> _questions = new();
    private Dictionary<int, int> _answers = new(); // questionIndex → choiceIndex
    private bool _quizSubmitted;
    private int _quizScore;

    // =====Lifecycle
    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated)
            Nav.NavigateTo("/auth/login");
    }

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated) return;

        _course = await CourseService.GetWithModulesAsync(CourseId);
        if (_course is null)
        {
            _isLoading = false;
            return;
        }

        _isEnrolled = await CourseService.IsEnrolledAsync(Auth.CurrentUser!.Id, CourseId);
        _isLoading = false;
    }

    // =====Event handlers
    private async Task SelectModule(int moduleId)
    {
        _activeModuleId = moduleId;
        _activeModule = _course?.Modules.FirstOrDefault(m => m.Id == moduleId);

        // Reset quiz state for the newly selected module
        _questions = new();
        _answers = new();
        _quizSubmitted = false;
        _quizScore = 0;
        _moduleMsg = null;

        if (_activeModule is not null)
            _questions = await QuizService.GetQuestionsForModuleAsync(moduleId);
    }

    private async Task EnrollAsync()
    {
        var success = await CourseService.EnrollAsync(Auth.CurrentUser!.Id, CourseId);
        if (success)
        {
            _isEnrolled = true;
            _enrollMsg = "You've enrolled! Good luck 🎉";
            _enrollMsgType = "success";
        }
        else
        {
            _enrollMsg = "Enrolment failed or you are already enrolled.";
            _enrollMsgType = "danger";
        }
    }

    private async Task MarkComplete()
    {
        if (!_isEnrolled) return;
        await CourseService.IncrementProgressAsync(Auth.CurrentUser!.Id, CourseId);
        _moduleMsg = "Module marked as complete ✓";
        _moduleMsgType = "success";
    }

    // =====Quiz helpers
    private void SelectAnswer(int questionIdx, int choiceIdx)
    {
        _answers[questionIdx] = choiceIdx;
    }

    private async Task SubmitQuiz()
    {
        _quizScore = 0;
        for (int i = 0; i < _questions.Count; i++)
        {
            if (_answers.GetValueOrDefault(i, -1) == _questions[i].CorrectIndex)
                _quizScore++;
        }
        _quizSubmitted = true;

        if (_activeModule is not null)
            await QuizService.SaveAttemptAsync(Auth.CurrentUser!.Id, _activeModule.Id, _quizScore, _questions.Count);
    }

    private void ResetQuiz()
    {
        _answers = new();
        _quizSubmitted = false;
        _quizScore = 0;
    }
}
