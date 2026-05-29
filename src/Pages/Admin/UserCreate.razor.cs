// UserCreate.razor.cs
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;

using PwnLearn.Services;


namespace PwnLearn.Pages.Admin;


public partial class UserCreate : ComponentBase
{
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    private readonly UserCreateModel _model = new();
    private string? _error;
    private bool _isSubmitting;

    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Nav.NavigateTo("/auth/login", replace: true);
            return;
        }
    }

    private async Task HandleCreate()
    {
        _isSubmitting = true;
        _error = null;

        var (ok, error) = await UserService.AdminCreateAsync(
            _model.Name, _model.Email, _model.Password, _model.Role, _model.IsActive);

        if (ok)
            Nav.NavigateTo("/admin/users?status=created");
        else
        {
            _error = error ?? "Failed to create user. Please try again.";
            _isSubmitting = false;
        }
    }

    private sealed class UserCreateModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Name must be 2–80 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(160)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Member|Admin)$", ErrorMessage = "Select a valid role.")]
        public string Role { get; set; } = "Member";

        public bool IsActive { get; set; } = true;
    }
}
