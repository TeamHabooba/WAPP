// ModuleCreate.razor.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using PwnLearn.Models;
using PwnLearn.Services;

namespace PwnLearn.Pages.Admin;

public partial class ModuleCreate : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IModuleService ModuleService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    [Parameter] public int CourseId { get; set; }

    private readonly ModuleFormModel _model = new();
    private string? _error;
    private bool _isSubmitting;

    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated || !Auth.IsAdmin)
            Nav.NavigateTo("/auth/login");
    }

    private async Task HandleCreate()
    {
        _isSubmitting = true;
        _error = null;

        var module = new Module
        {
            CourseId = CourseId,
            Title = _model.Title,
            Content = _model.Content,
            OrderIndex = 0  // assigned by ModuleService.CreateAsync
        };

        var ok = await ModuleService.CreateAsync(module);
        if (ok)
            Nav.NavigateTo($"/admin/courses/{CourseId}/modules?status=created");
        else
        {
            _error = "Failed to create module. Please try again.";
            _isSubmitting = false;
        }
    }

    private sealed class ModuleFormModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Title must be 2–200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required.")]
        [MinLength(10, ErrorMessage = "Content must be at least 10 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}
