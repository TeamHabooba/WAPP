// UserEdit.razor.cs
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;

using PwnLearn.Services;


namespace PwnLearn.Pages.Admin;


public partial class UserEdit : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    [Parameter] public int Id { get; set; }

    private UserEditModel? _model;
    private bool _isLoading = true;
    private bool _isSubmitting;
    private string? _error;
    private string? _success;

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Nav.NavigateTo("/auth/login", replace: true);
            return;
        }
        var user = await UserService.GetByIdAsync(Id);
        if (user is null)
        {
            _isLoading = false;
            return;
        }
        _model = new UserEditModel
        {
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive
        };
        _isLoading = false;
    }

    private async Task HandleUpdate()
    {
        if (_model is null) return;
        _isSubmitting = true;
        _error = null;
        _success = null;
        var user = await UserService.GetByIdAsync(Id);
        if (user is null)
        {
            _error = "User not found.";
            _isSubmitting = false;
            return;
        }
        // Check for duplicate email (excluding current user)
        var existing = await UserService.GetByEmailAsync(_model.Email.ToLower().Trim());
        if (existing is not null && existing.Id != Id)
        {
            _error = "This email is already in use by another account.";
            _isSubmitting = false;
            return;
        }
        user.Name = _model.Name.Trim();
        user.Email = _model.Email.ToLower().Trim();
        // Prevent admin from stripping their own privileges
        if (Id != Auth.CurrentUser!.Id)
        {
            user.Role = _model.Role;
            user.IsActive = _model.IsActive;
        }
        var ok = await UserService.UpdateAsync(user);
        if (ok)
            _success = "User updated successfully.";
        else
            _error = "Update failed. Please try again.";

        _isSubmitting = false;
    }

    private sealed class UserEditModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Name must be 2–80 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(160)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Member|Admin)$", ErrorMessage = "Select a valid role.")]
        public string Role { get; set; } = "Member";

        public bool IsActive { get; set; } = true;
    }
}
