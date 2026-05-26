// QuestionList.razor.cs
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using PwnLearn.Models;
using PwnLearn.Services;

namespace PwnLearn.Pages.Admin;

public partial class QuestionList : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IQuizService QuizService { get; set; } = default!;
    [Inject] private IModuleService ModuleService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    [Parameter] public int ModuleId { get; set; }

    [SupplyParameterFromQuery(Name = "status")]
    private string? Status { get; set; }

    private Module? _module;
    private List<QuizQuestion>? _questions;
    private bool _isLoading = true;
    private string? _feedback;
    private string _feedbackType = "success";
    private bool _showConfirm;
    private int _deleteId;

    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated || !Auth.IsAdmin)
            Nav.NavigateTo("/auth/login");
    }

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAdmin) return;

        _module = await ModuleService.GetByIdAsync(ModuleId);
        if (_module is null)
        {
            Nav.NavigateTo("/admin/courses");
            return;
        }

        await LoadQuestionsAsync();

        _feedback = Status switch
        {
            "created" => "Question created successfully.",
            "updated" => "Question updated successfully.",
            _ => null
        };
        _feedbackType = "success";
    }

    private void ConfirmDelete(int id, string text)
    {
        _deleteId = id;
        _showConfirm = true;
    }

    private async Task ExecuteDelete()
    {
        _showConfirm = false;
        var ok = await QuizService.DeleteQuestionAsync(_deleteId);
        _feedback = ok ? "Question deleted." : "Delete failed. Please try again.";
        _feedbackType = ok ? "success" : "danger";
        if (ok) await LoadQuestionsAsync();
    }

    private void CancelDelete() => _showConfirm = false;

    private async Task LoadQuestionsAsync()
    {
        _isLoading = true;
        _questions = await QuizService.GetQuestionsForModuleAsync(ModuleId);
        _isLoading = false;
    }

    private static string[] ParseChoices(string json)
    {
        try { return JsonSerializer.Deserialize<string[]>(json) ?? []; }
        catch { return []; }
    }
}
