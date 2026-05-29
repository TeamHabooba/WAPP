// QuestionEdit.razor.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

using Microsoft.AspNetCore.Components;

using PwnLearn.Services;


namespace PwnLearn.Pages.Admin;


public partial class QuestionEdit : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IQuizService QuizService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    [Parameter] public int ModuleId { get; set; }
    [Parameter] public int QuestionId { get; set; }

    private QuestionFormModel? _model;
    private bool _isLoading = true;
    private bool _isSubmitting;
    private string? _error;

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Nav.NavigateTo("/auth/login", replace: true);
            return;
        }
        var question = await QuizService.GetQuestionByIdAsync(QuestionId);
        if (question is null || question.ModuleId != ModuleId)
        {
            _isLoading = false;
            return;
        }
        var choices = DeserializeChoices(question.ChoicesJson);
        _model = new QuestionFormModel
        {
            QuestionText = question.QuestionText,
            Choices = [
                choices.ElementAtOrDefault(0) ?? string.Empty,
                choices.ElementAtOrDefault(1) ?? string.Empty,
                choices.ElementAtOrDefault(2) ?? string.Empty,
                choices.ElementAtOrDefault(3) ?? string.Empty
            ],
            CorrectIndex = question.CorrectIndex,
            Explanation = question.Explanation
        };
        _isLoading = false;
    }

    private async Task HandleUpdate()
    {
        if (_model is null) 
        {
            return; 
        }
        if (_model.Choices.Any(string.IsNullOrWhiteSpace))
        {
            _error = "All four choices must be filled in.";
            return;
        }
        _isSubmitting = true;
        _error = null;
        var question = await QuizService.GetQuestionByIdAsync(QuestionId);
        if (question is null)
        {
            _error = "Question not found.";
            _isSubmitting = false;
            return;
        }
        question.QuestionText = _model.QuestionText.Trim();
        question.ChoicesJson = JsonSerializer.Serialize(_model.Choices.Select(c => c.Trim()).ToArray());
        question.CorrectIndex = _model.CorrectIndex;
        question.Explanation = _model.Explanation.Trim();
        var ok = await QuizService.UpdateQuestionAsync(question);
        if (ok)
            Nav.NavigateTo($"/admin/modules/{ModuleId}/questions?status=updated");
        else
        {
            _error = "Update failed. Please try again.";
            _isSubmitting = false;
        }
    }

    private static string[] DeserializeChoices(string json)
    {
        try { return JsonSerializer.Deserialize<string[]>(json) ?? []; }
        catch { return []; }
    }

    private sealed class QuestionFormModel
    {
        [Required(ErrorMessage = "Question text is required.")]
        [MinLength(5, ErrorMessage = "Question must be at least 5 characters.")]
        public string QuestionText { get; set; } = string.Empty;
        public string[] Choices { get; set; } = ["", "", "", ""];
        [Range(0, 3)]
        public int CorrectIndex { get; set; } = 0;
        [Required(ErrorMessage = "Explanation is required.")]
        [MinLength(5, ErrorMessage = "Explanation must be at least 5 characters.")]
        public string Explanation { get; set; } = string.Empty;
    }
}
