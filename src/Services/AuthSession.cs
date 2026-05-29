// AuthSession.cs
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace PwnLearn.Services;


/// <summary>
/// Scoped service that holds the currently authenticated user for the Blazor Server circuit.
/// Replaces ASP.NET Core Identity for this project.
/// </summary>
public class AuthSession
{
    /// <summary>Session storage to save AuthSession after page refresh.</summary>
    private readonly ProtectedSessionStorage _storage;
    /// <summary> </summary>
    private const string Key = "auth_user_id";
    /// <summary>The currently logged-in user, or null if not authenticated.</summary>
    public Models.User? CurrentUser { get; private set; }

    /// <summary>True when a user is logged in.</summary>
    public bool IsAuthenticated => CurrentUser is not null;

    /// <summary>True when the logged-in user has the Admin role.</summary>
    public bool IsAdmin => CurrentUser?.Role == "Admin";

    /// <summary>True when the logged-in user has the Member role.</summary>
    public bool IsMember => CurrentUser?.Role == "Member";

    /// <summary>Flag to check if the AuthSession is still restoring after page reload.</summary>
    public bool IsRestoring { get; private set; } = true;

    // Fires whenever auth state changes so NavMenu can re-render
    public event Action? OnChange;

    public AuthSession(ProtectedSessionStorage storage)
    {
        _storage = storage;
    }

    /// <summary>
    /// Called once when the circuit starts — restores user from sessionStorage
    /// </summary>
    public async Task RestoreAsync(IUserService userService)
    {
        try
        {
            var result = await _storage.GetAsync<int>(Key);
            if (result.Success && result.Value > 0)
            {
                var user = await userService.GetByIdAsync(result.Value);
                if (user is not null)
                {
                    CurrentUser = user;
                }
            }
        }
        catch
        {
            // sessionStorage unavailable at prerender — just ignore
        }
        finally
        {
            IsRestoring = false;
            NotifyStateChanged();
        }
    }

    /// <summary>Sets the session user and notifies subscribers.</summary>
    public async Task LoginAsync(Models.User user)
    {
        CurrentUser = user;
        await _storage.SetAsync(Key, user.Id);
        NotifyStateChanged();
    }

    /// <summary>Clears the session and notifies subscribers.</summary>
    public async Task LogoutAsync()
    {
        CurrentUser = null;
        await _storage.DeleteAsync(Key);
        NotifyStateChanged();
    }

    /// <summary>Deprecated. Kept for backwards compatibility.</summary>
    public void Login(Models.User user) => _ = LoginAsync(user);

    /// <summary>Deprecated. Kept for backwards compatibility.</summary>
    public void Logout() => _ = LogoutAsync();

    private void NotifyStateChanged() => OnChange?.Invoke();
}
