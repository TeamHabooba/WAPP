Complete C# / Blazor code style guideline for the project.  
Refer to the project lead if you find anything unspecified. But please,  
**READ THIS GUIDELINE THOROUGHLY FIRST**.

## Contents
- [General Principles](#general-principles)
- [Indentation and Formatting](#indentation-and-formatting)
- [Naming Conventions](#naming-conventions)
- [Blazor Component Structure](#blazor-component-structure)
- [Code-Behind Files](#code-behind-files)
- [Namespaces and Usings](#namespaces-and-usings)
- [Comments](#comments)
- [Class and Member Order](#class-and-member-order)
- [Visibility and Access](#visibility-and-access)
- [Dependency Injection](#dependency-injection)
- [Error Handling](#error-handling)
- [Miscellaneous](#miscellaneous)
- [Quick Component Template](#quick-component-template)

---

### General Principles
- One component per file. A component `UserCard` → `UserCard.razor` + `UserCard.razor.cs`.
- Based on **Microsoft C# Coding Conventions** with project-specific extensions.
- Readability first. Prefer clarity over cleverness.
- Separate Blazor markup (`.razor`) from C# logic (`.razor.cs`) for all non-trivial components.
- Encoding: UTF-8.
- Line endings: CRLF (Windows). The `.editorconfig` file enforces this.

---

### Indentation and Formatting
- Use **spaces** over tabs.
- Indent: **4 spaces** (C# / .NET default).
- Spaces:
  - One space after keywords: `if (x == 0) { ... }`
  - Spaces around binary operators: `int total = price * quantity;`
  - No space before `(` in method calls: `service.GetUsers();`
- Blank lines: use one blank line to separate logical sections
  (between property groups, between constructor and methods, etc.).
- Do not add trailing whitespace.
- Maximum line length: **120 characters**. Break longer lines at a logical point.

---

### Naming Conventions

| Thing | Style | Example |
|---|---|---|
| Class / struct / enum / interface | `PascalCase` | `UserService`, `IUserService`, `UserRole` |
| Method | `PascalCase` | `GetUserById()`, `DeleteRecord()` |
| Property | `PascalCase` | `UserName`, `IsActive` |
| Local variable | `camelCase` | `userId`, `recordCount` |
| Private field | `_camelCase` | `_userService`, `_context` |
| Constant | `PascalCase` or `UPPER_SNAKE_CASE` | `MaxRetries`, `MAX_RETRIES` |
| Namespace | `PascalCase` | `namespace Wapp.Pages.Admin` |
| Enum value | `PascalCase` | `UserRole.Admin`, `Status.Active` |
| Blazor parameter | `PascalCase` | `[Parameter] public int UserId` |
| Blazor event callback | `PascalCase` + `Changed` suffix | `OnUserDeleted`, `ValueChanged` |

- Avoid Hungarian notation and prefixes like `m_name`, `strValue`, `bFlag`.
- Interface names must start with `I`: `IUserService`, `ILearningService`.

---

### Blazor Component Structure

Each `.razor` file should be organized in this order:

1. `@page` directive (if it is a routable page)
2. `@using` directives (only those not covered by `_Imports.razor`)
3. `@inject` directives
4. HTML / component markup
5. `@code { ... }` block — **only for simple components** (few properties, no business logic).
   For anything non-trivial, move all C# to the code-behind file.

```razor
@page "/admin/users"
@using Wapp.Models
@inject IUserService UserService
@inject NavigationManager Nav

<h2>Users</h2>

@if (_users is null) {
    <p>Loading...</p>
} else {
    <table>
        @foreach (var user in _users) {
            <tr><td>@user.Name</td></tr>
        }
    </table>
}
```

- Use `@if`, `@foreach`, `@switch` for conditional/iterative rendering — never raw C# in markup.
- Always use `@` prefix for binding: `@user.Name`, `@onclick`, `@bind`.
- Keep markup clean: extract repeated UI blocks into child components.

---

### Code-Behind Files

For all non-trivial pages and components, use a `.razor.cs` partial class:

```
Pages/Admin/UserList.razor      ← markup only
Pages/Admin/UserList.razor.cs   ← all C# logic
```

The code-behind file must:
- Be declared as `partial` and match the component class name exactly.
- Inherit from `ComponentBase` only if needed (usually implicit through the `.razor` file).
- Use `[Inject]` for dependency injection (instead of `@inject` in the razor file).

```csharp
// UserList.razor.cs
namespace Wapp.Pages.Admin;

public partial class UserList : ComponentBase
{
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    private List<User>? _users;

    protected override async Task OnInitializedAsync()
    {
        _users = await UserService.GetAllAsync();
    }
}
```

---

### Namespaces and Usings

- Use **file-scoped namespaces** (C# 10+):
```csharp
// Good
namespace Wapp.Pages.Admin;

public partial class UserList { ... }

// Bad
namespace Wapp.Pages.Admin
{
    public partial class UserList { ... }
}
```

- Add project-wide `using` statements to `_Imports.razor` (for Blazor) or a global using file
  (for C#). Do not repeat common usings in every file.
- Do **not** use `using static` globally. Use it locally and only when it genuinely improves readability.
- Group usings in this order, with one blank line between groups:
  1. System namespaces
  2. Microsoft / ASP.NET / Blazor namespaces
  3. Third-party namespaces
  4. Project namespaces

---

### Comments

- Use `//` for all inline and section comments. Avoid `/* */` blocks.
- File-level comment: add a one-line `// FileName.razor.cs` at the very top of each code-behind file:
```csharp
// UserList.razor.cs
namespace Wapp.Pages.Admin;
```
- Use XML doc comments (`/// <summary>`) on all public classes, methods, and properties in service
  and model files:
```csharp
/// <summary>
/// Returns all registered users from the database.
/// </summary>
public Task<List<User>> GetAllAsync() { ... }
```
- Use `// =====Section Name` banners to separate major sections inside long code-behind files:
```csharp
// =====Lifecycle methods

// =====Event handlers

// =====Private helpers
```
- Comment *why*, not *what*. Avoid restating the obvious:
```csharp
// Good: explains a non-obvious decision
// EF Core lazy-loading is disabled; load navigation properties explicitly
var users = await _context.Users.Include(u => u.Roles).ToListAsync();

// Bad: just restates the code
// Get all users
var users = await _context.Users.ToListAsync();
```
- Mark incomplete or unsafe spots with a short inline note:
```csharp
public async Task DeleteAsync(int id)
{
    // add authorization check
    var user = await _context.Users.FindAsync(id);
    ...
}
```

---

### Class and Member Order

Recommended order inside a class (top → bottom):

1. `private` fields (injected services and state)
2. `[Parameter]` properties
3. `[CascadingParameter]` properties
4. Public properties (non-parameter)
5. Lifecycle methods (`OnInitializedAsync`, `OnParametersSetAsync`, etc.) in lifecycle order
6. Event handlers (`HandleSubmit`, `OnDeleteClicked`, etc.)
7. Private helper methods

```csharp
public partial class UserList : ComponentBase
{
    // ===== Injected services
    [Inject] private IUserService UserService { get; set; } = default!;

    // ===== Parameters
    [Parameter] public int PageSize { get; set; } = 20;

    // ===== State
    private List<User>? _users;
    private bool _isLoading;

    // ===== Lifecycle
    protected override async Task OnInitializedAsync() { ... }

    // ===== Event handlers
    private async Task HandleDelete(int id) { ... }

    // ===== Helpers
    private string FormatDate(DateTime dt) => dt.ToString("dd MMM yyyy");
}
```

---

### Visibility and Access

- All fields are `private`. Always. No exceptions.
- Use `[Parameter]` for values passed in from a parent component — never `public` fields.
- Getters and setters for component state use `private` C# properties or fields.
- Never expose EF Core `DbContext` directly from a component. Always go through a service.
- Define all constant values as `const` or `static readonly`. Do not use `#define`.

---

### Dependency Injection

- Register all services in `Program.cs` via `builder.Services.Add...`.
- Use constructor injection in service classes:
```csharp
public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
}
```
- Use `[Inject]` in Blazor code-behind files (preferred over `@inject` for code-behind):
```csharp
[Inject] private IUserService UserService { get; set; } = default!;
```
- Never use `new` to instantiate a service. Always rely on DI.
- Register `DbContext` with `AddDbContext<AppDbContext>` and read the connection string from
  `appsettings.json`:
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

### Error Handling

- Do not use exceptions for control flow.
- Catch exceptions at the boundary (service layer or component) and surface them as user-readable
  messages — do not let unhandled exceptions bubble to the Blazor error UI.
- Use `try/catch` in service methods that perform database operations:
```csharp
public async Task<bool> DeleteAsync(int id)
{
    try
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity is null) return false;
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
    catch (DbUpdateException ex)
    {
        // log ex
        return false;
    }
}
```
- Expose operation results via `bool` return values or a simple result record — do not throw
  exceptions from service methods that the component must handle:
```csharp
// Good — explicit result
public record ServiceResult(bool Success, string? Error = null);

// Good — simple bool for operations that can only succeed or fail
public Task<bool> DeleteAsync(int id);

// Bad — throws from service, forces try/catch in every component
public async Task DeleteAsync(int id) { ... throw new Exception("Not found"); }
```
- Never silently swallow exceptions. At minimum, log them.
- Methods that are not yet implemented should throw `NotImplementedException`
  and be marked with `// TODO`:
```csharp
public Task<List<User>> SearchAsync(string query)
{
    // TODO: implement search
    throw new NotImplementedException();
}
```

---

### Miscellaneous

- Use `async`/`await` consistently — never mix `.Result` or `.Wait()` with async code.
- Always `await` EF Core async methods: `ToListAsync()`, `FindAsync()`, `SaveChangesAsync()`.
- Use `null` coalescing and null-conditional operators to keep code concise:
```csharp
var name = user?.Name ?? "Anonymous";
```
- Avoid magic numbers and strings. Use named constants or enum values:
```csharp
// Good
if (password.Length < MinPasswordLength) { ... }

// Bad
if (password.Length < 8) { ... }
```
- Prefer `var` when the type is obvious from the right-hand side:
```csharp
// Good
var users = await UserService.GetAllAsync();

// Bad (redundant type annotation)
List<User> users = await UserService.GetAllAsync();
```
- Do not use `var` when the type is not obvious:
```csharp
// Bad
var result = GetSomething(); // what type is result?
```

---

### Quick Component Template

`Pages/Admin/UserList.razor`:
```razor
@page "/admin/users"

<PageTitle>Manage Users</PageTitle>

<h2>Users</h2>

@if (_isLoading)
{
    <p>Loading...</p>
}
else if (_users is { Count: 0 })
{
    <p>No users found.</p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Email</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var user in _users!)
            {
                <tr>
                    <td>@user.Id</td>
                    <td>@user.Name</td>
                    <td>@user.Email</td>
                    <td>
                        <button @onclick="() => HandleDelete(user.Id)">Delete</button>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}

@if (_errorMessage is not null)
{
    <p class="text-danger">@_errorMessage</p>
}
```

`Pages/Admin/UserList.razor.cs`:
```csharp
// UserList.razor.cs
namespace Wapp.Pages.Admin;

public partial class UserList : ComponentBase
{
    // =====Injected services
    [Inject] private IUserService UserService { get; set; } = default!;
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // =====State
    private List<User>? _users;
    private bool _isLoading;
    private string? _errorMessage;

    // =====Lifecycle
    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        _users = await UserService.GetAllAsync();
        _isLoading = false;
    }

    // =====Event handlers
    private async Task HandleDelete(int id)
    {
        var success = await UserService.DeleteAsync(id);
        if (success)
        {
            _users = _users?.Where(u => u.Id != id).ToList();
        }
        else
        {
            _errorMessage = "Failed to delete user. Please try again.";
        }
    }
}
```
