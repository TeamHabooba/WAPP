// Logout.razor.cs
using Microsoft.AspNetCore.Components;
using PwnLearn.Services;

namespace PwnLearn.Pages.Auth;

public partial class Logout : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====Lifecycle
    protected override void OnInitialized()
    {
        Auth.Logout();
        Nav.NavigateTo("/");
    }
}
