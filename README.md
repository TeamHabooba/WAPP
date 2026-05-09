# WAPP
Web Applications Assignment (CT050-3-2-WAPP)  
(Group Assignment)  
100% of Semester Evaluation  
L-2 Group 30

## Contents
- [Team Members](#team-members)
- [Task Division](#task-division)
- [Assignment Question](#assignment-question)
- [Requirements](#requirements)
- [Guidelines for Team Members](#guidelines-for-team-members)
- [Q&A](#q--a)

## Team Members
| Student ID | Name |
|---|---|
| TP081705 | Kurapatkin Aliaksandr |
| TP082557 | Leon Frank Aminiel |
| TP084561 | Colin Subira Kwilabya |
| TP076126 | Shunto Matsumoto |

## Project Overview

This project implements a **Web-based Learning System** — a platform for registered and
non-registered users to discover and access digital learning resources.
The system is built with **.NET 8 / Blazor Server**, backed by a local **SQL Server** database.

The platform covers:
- Publicly accessible content for non-registered visitors.
- A **Member module** (registration + login required) for managing personal learning activities.
- An **Admin module** (login required) for managing website content and the database.

## Task Division

### Explanation
Each member is individually assessed on their own implemented pages and features
(see the marking rubric in [doc/AssignmentTask.pdf](./doc/AssignmentTask.pdf)).
Task boundaries follow the three main feature areas of the site.

### Modules

| Module | Assignee | Key Responsibility |
|---|---|---|
| Web Page Layout & Appearance + Navigation | Alex (TP081705) | Overall UI, CSS, Blazor layouts, routing |
| User Authentication & Authorization + Registration | Leo (TP082557) | Login, registration, role-based access |
| Dynamic Content + Member Module | Colin (TP084561) | Member dashboard, interactive features |
| CRUD operations + Admin Module | Shun (TP076126) | Admin panel, insert/update/delete records |

A detailed set of requirements per module is described in [doc/code-reqs.md](doc/code-reqs.md).

### Module Summaries

**Web Page Layout & Appearance** — Defines the global Blazor layout components, applies CSS
(external, internal, and inline where appropriate), ensures a consistent visual theme across all pages,
and implements the navigation structure.

**User Authentication & Authorization** — Implements the registration page for new members,
login/logout flow, and role-based route guarding for Member and Admin areas.

**Dynamic Content (Member Module)** — Delivers interactive learning resource pages accessible
to logged-in members: activity listings, self-assessments, and personal progress tracking.

**CRUD Operations (Admin Module)** — Provides the admin-only section for managing site content
and the database: creating, displaying, editing, and deleting records with proper form validation.

## Assignment Question
The full assignment PDF can be opened via [this link](./doc/AssignmentTask.pdf).

## Requirements
All [functional](doc/code-reqs.md#functional) and [non-functional](doc/code-reqs.md#non-functional)
project requirements are documented in [doc/code-reqs.md](doc/code-reqs.md).

Key constraints from the assignment:
- Built with **.NET technology** — Group 30 uses **Blazor Server (.NET 8)**.
- Must include interlinked pages, HTML5, CSS (external/internal/inline), and multimedia.
- Full CRUD database operations (insert, display, update, delete).
- Registration page, member module, and admin module — all login-protected where required.
- Form validation and logical navigation are mandatory.
- A local database is required (Group 30 uses SQL Server / LocalDB).

## Submission

- **Proposal Report deadline:** Week 7 via Moodle. **Submitted**
- **Final submission:** Complete website + Final Report via Moodle (exact deadline TBD by supervisor).
- **File format:** Submit as instructed on Moodle (website source + report document).

ZIP naming format (for reference only):  
`G30_TP081705_TP082557_TP084561_TP076126.zip`

## Guidelines for Team Members
> [!TIP]
> If after reading all `.md` guidelines and the [Q&A](#q--a) section you still have questions,
> research in this order:
> 1. ChatGPT / DeepSeek / Grok / Claude *(be aware of potential coding mistakes if not context-pretrained)*
> 2. Perplexity + Google
> 3. YouTube
> 4. Reddit
> 5. StackOverflow *(highly effective if you know how to search; also try GPT with web/search mode pointing to SO)*
> 6. Group Leader

For contribution rules, branching, code style, and other organizational guidelines — see [CONTRIBUTING.md](CONTRIBUTING.md).

### Toolchain / Framework
| Tool | Choice |
|---|---|
| Language | C# 12 |
| Framework | .NET 8 — Blazor Server |
| UI components | Blazor built-ins + custom CSS |
| Target platform | Windows 10/11 |
| IDE | Any IDE that supports .NET |
| IDE (recommended) | Visual Studio 2022 Community |
| Database | SQL Server / LocalDB |
| ORM | Entity Framework Core 8 |
| Version control | Git + GitHub (no CI pipeline) |
| Unit Testing | *(Pending supervisor clarification)* |

### Style and Naming Conventions
Microsoft C# Coding Conventions with project-specific extensions.  
Full guidelines: [doc/code-style.md](./doc/code-style.md)

### Documentation
Documentation can be done in either or both of the following ways:
1. Create a `.md` file in the `doc/` directory (no strict formatting required).
2. Write XML doc comments on each entity: classes, components, methods, properties.

See [doc/code-style.md#comments](./doc/code-style.md#comments) for what must be mentioned.

## Q & A

### Q1
> What .NET technology stack should we use?

**Answer:** The assignment requires any .NET C# technology. Group 30 will use Blazor Server (.NET 8).

### Q2
> Can we use a CSS framework (Bootstrap, Tailwind, etc.)?

**Answer:** *(Pending supervisor clarification)*

### Q3
> What database engine should we use?

**Answer:** The assignment requires a local database. Group 30 uses **SQL Server / LocalDB** via
**Entity Framework Core**.

### Q4
> Are unit tests required or do they count as creativity?

**Answer:** *(Pending supervisor clarification)*

### Q5
> Does "dynamic content" mean we must use JavaScript, or is Blazor's C# interactivity sufficient?

**Answer:** *(Pending supervisor clarification)*

### Q6
> Do all four members need separate login-protected modules, or are Member and Admin enough?

**Answer:** *(Pending supervisor clarification)*

### Q7
> Is the usage of a third-party component library (e.g., MudBlazor) permitted?

**Answer:** *(Pending supervisor clarification)*
