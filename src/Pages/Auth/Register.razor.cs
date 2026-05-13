// Register.razor.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using PwnLearn.Services;

namespace PwnLearn.Pages.Auth;

public partial class Register : ComponentBase
{
    // =====Injected services
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====State
    private readonly RegisterModel _model = new();
    private string? _errorMessage;
    private string? _successMessage;
    private bool _isSubmitting;
    private bool _registered;

    protected override void OnInitialized()
    {
        if (Auth.IsAuthenticated)
            Nav.NavigateTo("/member/dashboard");
    }

    // =====Event handlers
    private async Task HandleRegister()
    {
        _isSubmitting = true;
        _errorMessage = null;

        // Server-side password confirmation check
        if (_model.Password != _model.ConfirmPassword)
        {
            _errorMessage = "Passwords do not match.";
            _isSubmitting = false;
            return;
        }

        var (success, error) = await UserService.RegisterAsync(_model.Name, _model.Email, _model.Password);
        if (!success)
        {
            _errorMessage = error;
            _isSubmitting = false;
            return;
        }

        _successMessage = "Account created successfully! You can now log in.";
        _registered = true;
        _isSubmitting = false;
    }

    // =====Inner form model
    private sealed class RegisterModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Name must be 2–80 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
