# Project Requirements

This file contains a full explanation of what must be implemented and the applicable coding rules.
For coding style conventions, refer to [code-style.md](./code-style.md).

---

## Functional

### General
All modules must work together as one complete, navigable web application following the core user flow:
1. A visitor opens the site and browses public content.
2. A new user registers for an account.
3. A registered member logs in and accesses their personal learning dashboard.
4. A member interacts with dynamic learning content (discussions, assessments, simulations, etc.).
5. An admin logs in and manages website content and database records.
6. The admin performs full CRUD operations: creates, views, updates, and deletes records.
7. All forms validate input before submission; navigation remains consistent throughout.

---

### Shared / Common
- The application must consist of **interlinked pages** with a persistent navigation structure.
- Must use **HTML5** elements appropriately.
- Must demonstrate **CSS** usage (external stylesheet in `wwwroot/css/app.css`, internal styles
  via `<style>` blocks in `.razor` files where appropriate, and inline styles where justified).
- Must provide **good quality content** relevant to a digital learning platform.
- Must connect to a **local SQL Server / LocalDB** database via Entity Framework Core.
- Proper **file organization** and naming conventions must be followed throughout (see [code-style.md](./code-style.md)).

---

### Web Page Layout & Appearance Module
> Assignee: Alex (TP081705)

**Functional requirements:**
- Define the global Blazor `MainLayout` component used across all pages.
- Implement a `NavMenu` component with links to all major sections of the site.
- Apply a consistent visual theme using an external CSS file (`wwwroot/css/app.css`).
- Integrate multimedia elements (images, icons, or embedded media) appropriate to the learning platform theme.
- Ensure the layout is responsive and visually coherent across different screen sizes.

**Key features:**
- Consistent header, footer, and sidebar/navigation across all pages.
- Active link highlighting in the navigation menu.
- CSS applied through at least two methods (external + one of: internal or inline).

**Expected outputs:**
- `Components/Layout/MainLayout.razor`
- `Components/Layout/NavMenu.razor`
- `wwwroot/css/app.css`

---

### User Authentication & Authorization Module
> Assignee: Leo (TP082557)

**Functional requirements:**
- Implement a **registration page** for new users (name, email, password, confirmation).
- Implement a **login page** with credential validation.
- Implement **logout** functionality.
- Protect Member and Admin routes — unauthenticated users must be redirected to login.
- Enforce **role-based access**: Members cannot access Admin pages, and vice versa.
- Validate all registration and login forms (client-side and/or server-side).

**Key features:**
- Secure password handling (hashed storage — do not store plaintext passwords).
- Meaningful error messages for invalid credentials or duplicate registration.
- Persistent authentication state across page navigations within the session.

**Expected outputs:**
- `Pages/Auth/Login.razor` + code-behind
- `Pages/Auth/Register.razor` + code-behind
- Authentication state provider or service integration
- Role-based route guards

---

### Dynamic Content — Member Module
> Assignee: Colin (TP084561)

**Functional requirements:**
- Require a valid member login to access all pages in this module.
- Display a **member dashboard** summarizing the user's learning activities.
- Provide at least one **interactive learning resource** (e.g., self-assessment quiz,
  discussion thread, progress tracker, or simulation).
- Dynamically load and display content from the database.
- Allow members to manage their own activities (e.g., enroll in, view, or mark content as complete).

**Key features:**
- All content is loaded dynamically from the database — no hardcoded static pages.
- Interactive elements that respond to user actions without full page reloads (Blazor interactivity).
- Real-time feedback for user interactions (e.g., form submission success/error).

**Expected outputs:**
- `Pages/Member/Dashboard.razor` + code-behind
- At least one interactive member feature page under `Pages/Member/`
- Corresponding service methods for member-specific data

---

### CRUD Operations — Admin Module
> Assignee: Shun (TP076126)

**Functional requirements:**
- Require a valid admin login to access all pages in this module.
- Provide a management interface for **at least one content type** (e.g., learning resources,
  users, categories).
- Implement all four CRUD operations:
  - **Insert:** Add new records via a validated form.
  - **Display:** List all existing records in a table or structured view.
  - **Update/Modify:** Edit existing records via a pre-populated form.
  - **Delete:** Remove records with a confirmation step.
- Validate all admin forms before committing changes to the database.

**Key features:**
- Full insert, update, and delete operations with validations.
- Confirmation dialog before destructive operations (delete).
- Feedback messages on successful or failed operations.
- Secure: only admins can reach these pages.

**Expected outputs:**
- `Pages/Admin/` directory with list, create, edit, and delete pages
- Corresponding service methods for admin-level data operations

---

## Non-functional

### Project Structure
- Separate Blazor component logic using **code-behind files** (`.razor` + `.razor.cs`) for all
  non-trivial pages.
- Keep **service interfaces** in `Services/I<Name>Service.cs` and implementations in `Services/<Name>Service.cs`.
- Keep all **EF Core entity models** in `Models/`.
- Keep the **DbContext** in `Data/AppDbContext.cs`.
- Register all services in `Program.cs` via dependency injection — do not instantiate services manually.

### Database
- Use **Entity Framework Core** with Code First migrations.
- All schema changes must go through migrations (`dotnet ef migrations add`).
  Do not modify the database schema manually.
- Connection string must be in `appsettings.json` — never hardcoded.

### Security
- Never store passwords in plaintext. Use a hashing mechanism (e.g., `BCrypt` or ASP.NET Core Identity).
- Always validate user input on the server side, regardless of client-side validation.
- Do not expose admin routes to non-admin users. Use `[Authorize(Roles = "Admin")]` or equivalent.

### File and Scope Discipline
- Only modify files within your own assigned directory (see [CONTRIBUTING.md](../CONTRIBUTING.md)).
- Never rewrite, delete, or create files outside your directory.
- If an error is found in another member's files, contact the responsible member immediately.

### Coding Style
- Follow all rules defined in [code-style.md](./code-style.md).

### Team Conduct
> [!NOTE]
> This is a brief version. See [CONTRIBUTING.md](../CONTRIBUTING.md) for a full explanation.
- Attend all meetings. Only critical meetings will be held offline.
- Complete all tasks on time.
- Failure to follow the two rules above may result in removal from the group.
- If you have a reason for absence or deadline failure, notify the team in the WhatsApp group chat in advance.
