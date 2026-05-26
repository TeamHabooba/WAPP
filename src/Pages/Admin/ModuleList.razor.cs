// ModuleList.razor.cs
using Microsoft.AspNetCore.Components;
using PwnLearn.Models;
using PwnLearn.Services;

namespace PwnLearn.Pages.Admin;

public partial class ModuleList : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IModuleService ModuleService { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    [Parameter] public int CourseId { get; set; }

    [SupplyParameterFromQuery(Name = "status")]
    private string? Status { get; set; }

    private Course? _course;
    private List<Module>? _modules;
    private bool _isLoading = true;
    private string? _feedback;
    private string _feedbackType = "success";
    private bool _showConfirm;
    private int _deleteId;
    private string _deleteTitle = string.Empty;

    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated || !Auth.IsAdmin)
            Nav.NavigateTo("/auth/login");
    }

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAdmin) return;

        _course = await CourseService.GetByIdAsync(CourseId);
        if (_course is null)
        {
            Nav.NavigateTo("/admin/courses");
            return;
        }

        await LoadModulesAsync();

        _feedback = Status switch
        {
            "created" => "Module created successfully.",
            "updated" => "Module updated successfully.",
            _ => null
        };
        _feedbackType = "success";
    }

    private void ConfirmDelete(int id, string title)
    {
        _deleteId = id;
        _deleteTitle = title;
        _showConfirm = true;
    }

    private async Task ExecuteDelete()
    {
        _showConfirm = false;
        var ok = await ModuleService.DeleteAsync(_deleteId);
        _feedback = ok ? $"Module '{_deleteTitle}' deleted." : "Delete failed. Please try again.";
        _feedbackType = ok ? "success" : "danger";
        if (ok) await LoadModulesAsync();
    }

    private void CancelDelete() => _showConfirm = false;

    private async Task LoadModulesAsync()
    {
        _isLoading = true;
        _modules = await ModuleService.GetByCourseAsync(CourseId);
        _isLoading = false;
    }
}
