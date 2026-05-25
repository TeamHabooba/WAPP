// QuestionCreate.razor.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using PwnLearn.Models;
using PwnLearn.Services;

namespace PwnLearn.Pages.Admin;

public partial class QuestionCreate : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IQuizService QuizService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    [Parameter] public int ModuleId { get; set; }

    private readonly QuestionFormModel _model = new();
    private string? _error;
    private bool _isSubmitting;

    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated || !Auth.IsAdmin)
            Nav.NavigateTo("/auth/login");
    }

    private async Task HandleCreate()
    {
        if (_model.Choices.Any(string.IsNullOrWhiteSpace))
        {
            _error = "All four choices must be filled in.";
            return;
        }

        _isSubmitting = true;
        _error = null;

        var question = new QuizQuestion
        {
            ModuleId = ModuleId,
            QuestionText = _model.QuestionText.Trim(),
            ChoicesJson = JsonSerializer.Serialize(_model.Choices.Select(c => c.Trim()).ToArray()),
            CorrectIndex = _model.CorrectIndex,
            Explanation = _model.Explanation.Trim()
        };

        var ok = await QuizService.CreateQuestionAsync(question);
        if (ok)
            Nav.NavigateTo($"/admin/modules/{ModuleId}/questions?status=created");
        else
        {
            _error = "Failed to create question. Please try again.";
            _isSubmitting = false;
        }
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
