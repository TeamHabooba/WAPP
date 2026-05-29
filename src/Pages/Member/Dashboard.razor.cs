// Dashboard.razor.cs
using Microsoft.AspNetCore.Components;

using PwnLearn.Models;
using PwnLearn.Services;


namespace PwnLearn.Pages.Member;


public partial class Dashboard : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private IQuizService QuizService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====State
    private List<Enrollment> _enrollments = new();
    private List<QuizAttempt> _quizAttempts = new();
    private int _totalCompleted;
    private string _bestScore = "—";
    private bool _isLoading = true;

    // =====Lifecycle

    /// <summary>
    /// Deprecated.
    /// </summary>
    //protected override void OnInitialized() => _ = OnInitializedAsync();

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Nav.NavigateTo("/auth/login");
        }
        var userId = Auth.CurrentUser!.Id;
        _enrollments = await CourseService.GetUserEnrollmentsAsync(userId);
        _quizAttempts = await QuizService.GetUserAttemptsAsync(userId);
        _totalCompleted = _enrollments.Sum(e => e.CompletedModules);
        if (_quizAttempts.Count > 0)
        {
            var best = _quizAttempts.OrderByDescending(a => (double)a.Score / a.TotalQuestions).First();
            _bestScore = $"{best.Score}/{best.TotalQuestions}";
        }
        _isLoading = false;
    }
}
