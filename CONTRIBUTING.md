# Contributing to WAPP

This file consolidates all organizational and workflow guidelines for Group 30.
Every team member is expected to read and follow this document before making any contribution.

---

## Table of Contents
- [Team Contacts](#team-contacts)
- [Directory Structure](#directory-structure)
- [Git Workflow](#git-workflow)
- [Pull Request Rules](#pull-request-rules)
- [Code Ownership](#code-ownership)
- [Meetings](#meetings)
- [Deadlines](#deadlines)
- [Grading Overview](#grading-overview)

---

## Team Contacts

| Role | Name | Student ID |
|---|---|---|
| Group Leader | Kurapatkin Aliaksandr | TP081705 |
| Member | Leon Frank Aminiel | TP082557 |
| Member | Colin Subira Kwilabya | TP084561 |
| Member | Shunto Matsumoto | TP076126 |

For urgent matters, use the WhatsApp group chat.  
For code-related issues, open a GitHub issue or message the responsible member directly.

---

## Directory Structure

```
src/
├── Components/         # Shared Blazor components — maintained by Alex
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   └── Shared/
├── Pages/
│   ├── Auth/           # Leo (TP082557) — login, registration, logout
│   ├── Member/         # Colin (TP084561) — member dashboard and activities
│   └── Admin/          # Shun (TP076126) — admin CRUD panel
├── Services/           # Service interfaces and implementations
├── Models/             # EF Core entity models
├── Data/               # DbContext and migrations
├── wwwroot/
│   ├── css/
│   │   └── app.css     # Global stylesheet — maintained by Alex
│   └── assets/
├── Program.cs          # App entry point — maintained by Alex
└── appsettings.json

doc/
├── AssignmentTask.pdf
├── code-reqs.md
└── code-style.md
```

> [!IMPORTANT]
> Each member works **only** within their own directory.
> Do not create, edit, or delete files outside your assigned directory.
> If you find a bug in another member's code, report it to them directly — do not fix it yourself.

---

## Git Workflow

### Branches
- `main` — stable, submission-ready code only. Direct pushes are prohibited.
- `<your-name>` — your personal working branch.

**Example branch names:**
```
alex
leo
colin
shun
```

### Commit Messages
Follow this format:
```
[module] Short imperative description

Optional longer body if needed.
```

**Examples:**
```
[layout] Add responsive NavMenu with active link highlight
[auth] Implement registration page with validation
[member] Add activity listing component
[admin] Add delete confirmation dialog for records
[shared] Extract UserCard into reusable component
```

- Use present tense: "Add feature" not "Added feature".
- Keep the first line under 72 characters.
- Reference issue numbers where relevant: `Fixes #12`.

---

## Pull Request Rules

1. **Never push directly to `main`.** Always open a Pull Request (PR).
2. Before opening a PR, make sure the project builds cleanly with no errors or warnings.
3. Remove any temporary test pages or debug output before submitting a PR.
4. PR title must follow the same format as commit messages.
5. The team leader must review and approve before merging to `main`.
6. Describe what your PR does and any known limitations in the PR description.

---

## Code Ownership

| Directory / File | Owner | Contact if broken |
|---|---|---|
| `src/Components/` | Alex (TP081705) | Alex |
| `src/Pages/Auth/` | Leo (TP082557) | Leo |
| `src/Pages/Member/` | Colin (TP084561) | Colin |
| `src/Pages/Admin/` | Shun (TP076126) | Shun |
| `src/Services/`, `src/Models/`, `src/Data/` | Alex (TP081705) | Alex |
| `src/Program.cs`, `src/appsettings.json` | Alex (TP081705) | Alex |
| `src/wwwroot/css/app.css` | Alex (TP081705) | Alex |
| `doc/` | All (read-only for members) | Alex |

If an error originates in a file that is not yours, **contact the owner ASAP** instead of modifying it.

---

## Meetings

- Attendance at all meetings is **mandatory**.
- Only critically important meetings will be held offline; routine syncs are online.
- If you cannot attend, notify the WhatsApp group **before** the meeting, not after.
- Unexplained absences count against your standing in the group.

---

## Deadlines

| Milestone | Date |
|---|---|
| Proposal Report submission to Moodle | Week 7 |
| Final website + Final Report submission | TBD by supervisor |

> [!WARNING]
> Late submissions may result in mark deductions or zero marks per APU policy.

---

## Grading Overview

Total: **100%** split equally between Documentation and Implementation.

### Documentation (50%) — Group criteria

| # | Criterion | Marks |
|---|---|---|
| 1 | Introduction | 10% |
| 2 | Storyboard & Requirement Specification | 10% |
| 3 | Design & Modelling | 10% |
| 4 | Implementation & Discussion | 10% |
| 5 | Conclusion, Document Styles & Formatting | 10% |

### Implementation / Website (50%) — Individual criteria

| # | Criterion | Marks |
|---|---|---|
| 1 | Web Page Layout & Appearance | 10% |
| 2 | User Authentication & Authorization | 10% |
| 3 | Dynamic Content | 10% |
| 4 | Insert, Update & Delete Records | 10% |
| 5 | Form Validation, Navigation & Usability | 10% |

Key grading factors:
- Correct and functional implementation of all required website criteria.
- Code readability, meaningful names, comments, and consistent style.
- Quality and depth of documentation relative to your implemented features.
- Security: proper authentication, authorization, and input validation.
