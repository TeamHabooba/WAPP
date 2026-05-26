// AdminDashboard.razor.cs
using Microsoft.AspNetCore.Components;
using PwnLearn.Services;

namespace PwnLearn.Pages.Admin;

public partial class AdminDashboard : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====Lifecycle
    protected override void OnInitialized()
    {
        if (!Auth.IsAuthenticated || !Auth.IsAdmin)
            Nav.NavigateTo("/auth/login");
    }
}
