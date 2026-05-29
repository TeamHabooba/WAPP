// UserList.razor.cs
using Microsoft.AspNetCore.Components;

using PwnLearn.Models;
using PwnLearn.Services;


namespace PwnLearn.Pages.Admin;


public partial class UserList : ComponentBase
{
    // =====Injected services
    [Inject] private AuthSession Auth { get; set; } = default!;
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====Query parameters
    [SupplyParameterFromQuery(Name = "status")]
    private string? Status { get; set; }

    // =====State
    private List<User>? _users;
    private string _search = string.Empty;
    private bool _isLoading = true;
    private string? _feedback;
    private string _feedbackType = "success";

    // Delete dialog state
    private bool _showConfirm;
    private int _deleteId;
    private string _deleteName = string.Empty;

    // =====Computed
    private List<User> Filtered => (_users ?? new())
        .Where(u => string.IsNullOrWhiteSpace(_search) ||
                    u.Name.Contains(_search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(_search, StringComparison.OrdinalIgnoreCase))
        .ToList();

    // =====Lifecycle
    protected override async Task OnInitializedAsync()
    {
        if (!Auth.IsAuthenticated)
        {
            Nav.NavigateTo("/auth/login", replace: true);
            return;
        }
        await LoadUsersAsync();
        _feedback = Status switch
        {
            "created" => "User created successfully.",
            _ => null
        };
        _feedbackType = "success";
    }

    // =====Event handlers
    private async Task ToggleUser(int id)
    {
        if (id == Auth.CurrentUser?.Id)
        {
            _feedback = "You cannot deactivate your own admin account.";
            _feedbackType = "danger";
            return;
        }

        var ok = await UserService.ToggleActiveAsync(id);
        if (ok)
        {
            await LoadUsersAsync();
            _feedback = "User status updated.";
            _feedbackType = "success";
        }
        else
        {
            _feedback = "Failed to update status.";
            _feedbackType = "danger";
        }
    }

    private void ConfirmDelete(int id, string name)
    {
        _deleteId = id;
        _deleteName = name;
        _showConfirm = true;
    }

    private async Task ExecuteDelete()
    {
        _showConfirm = false;
        var ok = await UserService.DeleteAsync(_deleteId);
        if (ok)
        {
            _feedback = $"User '{_deleteName}' deleted.";
            _feedbackType = "success";
            await LoadUsersAsync();
        }
        else
        {
            _feedback = "Delete failed. Please try again.";
            _feedbackType = "danger";
        }
    }

    private void CancelDelete() => _showConfirm = false;

    // =====Private helpers
    private async Task LoadUsersAsync()
    {
        _isLoading = true;
        _users = await UserService.GetAllAsync();
        _isLoading = false;
    }
}
