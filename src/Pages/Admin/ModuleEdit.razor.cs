// ModuleEdit.razor.cs
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;

using PwnLearn.Services;


namespace PwnLearn.Pages.Admin;

public partial class ModuleEdit : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IModuleService ModuleService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    [Parameter] public int CourseId { get; set; }
    [Parameter] public int ModuleId { get; set; }

    private ModuleFormModel? _model;
    private bool _isLoading = true;
    private bool _isSubmitting;
    private string? _error;
    private string? _success;

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

        var module = await ModuleService.GetByIdAsync(ModuleId);
        if (module is null || module.CourseId != CourseId)
        {
            _isLoading = false;
            return;
        }

        _model = new ModuleFormModel
        {
            Title = module.Title,
            Content = module.Content,
            OrderIndex = module.OrderIndex
        };
        _isLoading = false;
    }

    private async Task HandleUpdate()
    {
        if (_model is null) return;
        _isSubmitting = true;
        _error = null;
        _success = null;

        var module = await ModuleService.GetByIdAsync(ModuleId);
        if (module is null)
        {
            _error = "Module not found.";
            _isSubmitting = false;
            return;
        }

        module.Title = _model.Title;
        module.Content = _model.Content;
        module.OrderIndex = _model.OrderIndex;

        var ok = await ModuleService.UpdateAsync(module);
        if (ok)
            Nav.NavigateTo($"/admin/courses/{CourseId}/modules?status=updated");
        else
        {
            _error = "Update failed. Please try again.";
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

        [Range(1, 999, ErrorMessage = "Order must be between 1 and 999.")]
        public int OrderIndex { get; set; } = 1;
    }
}
