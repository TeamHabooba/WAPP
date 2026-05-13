// AuthSession.cs
namespace PwnLearn.Services;

/// <summary>
/// Scoped service that holds the currently authenticated user for the Blazor Server circuit.
/// Replaces ASP.NET Core Identity for this project.
/// </summary>
public class AuthSession
{
    /// <summary>The currently logged-in user, or null if not authenticated.</summary>
    public Models.User? CurrentUser { get; private set; }

    /// <summary>True when a user is logged in.</summary>
    public bool IsAuthenticated => CurrentUser is not null;

    /// <summary>True when the logged-in user has the Admin role.</summary>
    public bool IsAdmin => CurrentUser?.Role == "Admin";

    /// <summary>True when the logged-in user has the Member role.</summary>
    public bool IsMember => CurrentUser?.Role == "Member";

    // Fires whenever auth state changes so NavMenu can re-render
    public event Action? OnChange;

    /// <summary>Sets the session user and notifies subscribers.</summary>
    public void Login(Models.User user)
    {
        CurrentUser = user;
        NotifyStateChanged();
    }

    /// <summary>Clears the session and notifies subscribers.</summary>
    public void Logout()
    {
        CurrentUser = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
