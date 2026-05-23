// CourseEdit.razor.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using PwnLearn.Services;

namespace PwnLearn.Pages.Admin;

public partial class CourseEdit : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====Parameters
    [Parameter] public int Id { get; set; }

    // =====State
    private CourseFormModel? _model;
    private bool _isLoading = true;
    private bool _isSubmitting;
    private string? _error;
    private string? _success;

    // =====Lifecycle
    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated || !Auth.IsAdmin)
            Nav.NavigateTo("/auth/login");
    }

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAdmin) return;

        var course = await CourseService.GetByIdAsync(Id);
        if (course is null)
        {
            _isLoading = false;
            return;
        }

        _model = new CourseFormModel
        {
            Title = course.Title,
            Description = course.Description,
            Category = course.Category,
            Difficulty = course.Difficulty,
            IconEmoji = course.IconEmoji,
            IsPublished = course.IsPublished
        };
        _isLoading = false;
    }

    // =====Event handlers
    private async Task HandleUpdate()
    {
        if (_model is null) return;
        _isSubmitting = true;
        _error = null;
        _success = null;

        var course = await CourseService.GetByIdAsync(Id);
        if (course is null)
        {
            _error = "Course not found.";
            _isSubmitting = false;
            return;
        }

        course.Title = _model.Title;
        course.Description = _model.Description;
        course.Category = _model.Category;
        course.Difficulty = _model.Difficulty;
        course.IconEmoji = string.IsNullOrWhiteSpace(_model.IconEmoji) ? "🔐" : _model.IconEmoji;
        course.IsPublished = _model.IsPublished;

        var ok = await CourseService.UpdateAsync(course);
        if (ok)
        {
            _success = "Course updated successfully.";
        }
        else
        {
            _error = "Update failed. Please try again.";
        }
        _isSubmitting = false;
    }

    // =====Inner form model
    private sealed class CourseFormModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Title must be 3–120 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be 10–500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        [RegularExpression("^(Fundamentals|Tools|CTF)$", ErrorMessage = "Select a valid category.")]
        public string Category { get; set; } = string.Empty;

        [RegularExpression("^(Beginner|Intermediate|Advanced)$", ErrorMessage = "Select a valid difficulty.")]
        public string Difficulty { get; set; } = "Beginner";

        [StringLength(8)]
        public string IconEmoji { get; set; } = "🔐";

        public bool IsPublished { get; set; } = true;
    }
}
