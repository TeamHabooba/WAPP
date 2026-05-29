// CourseCreate.razor.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using PwnLearn.Models;
using PwnLearn.Services;

namespace PwnLearn.Pages.Admin;

public partial class CourseCreate : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private ICourseService CourseService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====State
    private readonly CourseFormModel _model = new();
    private string? _error;
    private bool _isSubmitting;

    // =====Lifecycle
    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Nav.NavigateTo("/auth/login", replace: true);
            return;
        }
    }

    // =====Event handlers
    private async Task HandleCreate()
    {
        _isSubmitting = true;
        _error = null;

        var course = new Course
        {
            Title = _model.Title,
            Description = _model.Description,
            Category = _model.Category,
            Difficulty = _model.Difficulty,
            IconEmoji = string.IsNullOrWhiteSpace(_model.IconEmoji) ? "🔐" : _model.IconEmoji,
            IsPublished = _model.IsPublished,
            CreatedAt = DateTime.UtcNow
        };

        var success = await CourseService.CreateAsync(course);
        if (success)
            Nav.NavigateTo("/admin/courses?status=created");
        else
        {
            _error = "Failed to create course. Please try again.";
            _isSubmitting = false;
        }
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
