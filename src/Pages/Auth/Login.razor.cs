// Login.razor.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using PwnLearn.Services;

namespace PwnLearn.Pages.Auth;

public partial class Login : ComponentBase
{
    // =====Injected services
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====State
    private readonly LoginModel _model = new();
    private string? _errorMessage;
    private bool _isSubmitting;
    private bool _submitted;

    // =====Lifecycle
    protected override void OnInitialized()
    {
        // Redirect already-authenticated users away from login
        if (Auth.IsAuthenticated)
            RedirectToHome();
    }

    // =====Event handlers
    private async Task HandleLogin()
    {
        _submitted = true;
        _isSubmitting = true;
        _errorMessage = null;
        var user = await UserService.AuthenticateAsync(_model.Email, _model.Password);
        if (user is null)
        {
            _errorMessage = "Invalid email or password. Please try again.";
            _isSubmitting = false;
            return;
        }
        await Auth.LoginAsync(user);
        RedirectToHome();
    }

    // =====Private helpers
    private void RedirectToHome()
    {
        if (Auth.IsAdmin)
            Nav.NavigateTo("/admin");
        else
            Nav.NavigateTo("/member/dashboard");
    }

    // =====Inner form model
    private sealed class LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
