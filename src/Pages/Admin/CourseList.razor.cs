// CourseList.razor.cs
using Microsoft.AspNetCore.Components;

using PwnLearn.Models;
using PwnLearn.Services;


namespace PwnLearn.Pages.Admin;

public partial class CourseList : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====Query parameters
    [SupplyParameterFromQuery(Name = "status")]
    private string? Status { get; set; }

    // =====State
    private List<Course>? _courses;
    private bool _isLoading = true;
    private string? _feedback;
    private string _feedbackType = "success";

    // Delete dialog state
    private bool _showConfirm;
    private int _deleteId;
    private string _deleteTitle = string.Empty;

    // =====Lifecycle
    /// <summary>
    /// Deprecated.
    /// </summary>
    //protected override void OnInitialized();

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Nav.NavigateTo("/auth/login", replace: true);
            return;
        }
        await LoadCoursesAsync();
        if (Status == "created")
        {
            _feedback = "Course created successfully.";
            _feedbackType = "success";
        }
    }

    // =====Event handlers
    private void ConfirmDelete(int id, string title)
    {
        _deleteId = id;
        _deleteTitle = title;
        _showConfirm = true;
    }

    private async Task ExecuteDelete()
    {
        _showConfirm = false;
        var success = await CourseService.DeleteAsync(_deleteId);
        if (success)
        {
            _feedback = $"Course '{_deleteTitle}' deleted.";
            _feedbackType = "success";
            await LoadCoursesAsync();
        }
        else
        {
            _feedback = "Delete failed. Please try again.";
            _feedbackType = "danger";
        }
    }

    private void CancelDelete()
    {
        _showConfirm = false;
    }

    // =====Private helpers
    private async Task LoadCoursesAsync()
    {
        _isLoading = true;
        _courses = await CourseService.GetAllAsync();
        _isLoading = false;
    }
}
