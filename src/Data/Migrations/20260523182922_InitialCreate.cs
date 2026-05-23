using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PwnLearn.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IconEmoji = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedModules = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    AttemptedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChoicesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectIndex = table.Column<int>(type: "int", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Difficulty", "IconEmoji", "IsPublished", "Title" },
                values: new object[,]
                {
                    { 1, "Fundamentals", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Core concepts of information security: CIA triad, threat landscape, attack surfaces, and defence strategies.", "Beginner", "🛡️", true, "Cybersecurity Fundamentals" },
                    { 2, "Tools", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Get comfortable with Kali Linux — the industry-standard penetration-testing distribution.", "Beginner", "🐉", true, "Kali Linux Essentials" },
                    { 3, "Tools", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Learn to use Metasploit for vulnerability assessment and controlled exploitation in lab environments.", "Intermediate", "⚡", true, "Metasploit Framework" },
                    { 4, "CTF", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Introduction to Capture-the-Flag competitions: web exploitation, reverse engineering, and cryptography.", "Intermediate", "🚩", true, "CTF Challenges & Strategy" },
                    { 5, "Fundamentals", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Understand the legal and ethical framework around penetration testing and vulnerability reporting.", "Beginner", "⚖️", true, "Ethical Hacking & Responsible Disclosure" },
                    { 6, "Tools", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Master Nmap, Wireshark, and passive OSINT techniques for network discovery.", "Advanced", "🔍", true, "Network Reconnaissance" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "Name", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@pwnlearn.io", true, "Admin", "$2a$11$cnoswt/QTctj7ElAQUC6xezDCqu9TGoAbcdaJby0kDnS05kAgPvMG", "Admin" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "demo@pwnlearn.io", true, "Demo Member", "$2a$11$kIZ9znlLyQm9NdMPkMb41OecQPMch8UCw1.m.uyEo4naTBK0ZpH26", "Member" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "Id", "CompletedModules", "CourseId", "EnrolledAt", "UserId" },
                values: new object[] { 1, 1, 1, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2 });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Content", "CourseId", "OrderIndex", "Title" },
                values: new object[,]
                {
                    { 1, "Confidentiality, Integrity, and Availability form the cornerstone of information security. Every security control can be mapped back to protecting one or more of these three properties. Confidentiality prevents unauthorised disclosure; Integrity ensures data is not tampered with; Availability guarantees that systems and data are accessible when needed.", 1, 1, "The CIA Triad" },
                    { 2, "Understanding who attacks systems and why is essential for building effective defences. Threat actors range from script kiddies and hacktivists to nation-state APT groups. Motivations include financial gain, espionage, ideological causes, and notoriety. Mapping actors to the MITRE ATT&CK framework helps defenders anticipate tactics.", 1, 2, "Threat Actors & Motivations" },
                    { 3, "An attack surface is the sum of all points where an attacker could try to enter or extract data from a system. Common vectors include phishing, unpatched software, weak credentials, and misconfigured cloud resources. Reducing the attack surface through least privilege, patch management, and network segmentation is a primary defensive strategy.", 1, 3, "Attack Surfaces & Vectors" },
                    { 4, "Kali Linux can be installed as a VM (VirtualBox/VMware), run live from USB, or deployed in WSL on Windows. After installation, update the package list with `sudo apt update && sudo apt upgrade`. The default toolset includes over 600 security tools pre-configured and ready to use.", 2, 1, "Installing & Configuring Kali" },
                    { 5, "Mastery of the Linux terminal is non-negotiable for penetration testers. Key commands: `ls`, `cd`, `cat`, `grep`, `find`, `chmod`, `netstat`, `ps`, and `kill`. Piping (`|`) and redirection (`>`, `>>`) allow powerful one-liners. Practice building compound commands to automate reconnaissance tasks.", 2, 2, "Essential Terminal Commands" },
                    { 6, "Metasploit Framework consists of modules: Exploits, Payloads, Auxiliaries, Encoders, and Post-exploitation modules. The `msfconsole` is the primary interface. Understanding the difference between a bind shell and a reverse shell payload is fundamental before running any module.", 3, 1, "Metasploit Architecture" },
                    { 7, "Using a controlled Metasploitable VM, we practice the full exploitation cycle: `search`, `use`, `set RHOSTS`, `set PAYLOAD`, `run`. Always confirm you have written permission before testing any system. Document your findings in a structured report with steps to reproduce and suggested remediation.", 3, 2, "Your First Exploit (Lab)" }
                });

            migrationBuilder.InsertData(
                table: "QuizQuestions",
                columns: new[] { "Id", "ChoicesJson", "CorrectIndex", "Explanation", "ModuleId", "QuestionText" },
                values: new object[,]
                {
                    { 1, "[\"Confidentiality\",\"Integrity\",\"Availability\",\"Authentication\"]", 2, "Availability guarantees that systems and data remain accessible to authorised users. DDoS attacks typically target availability.", 1, "Which element of the CIA triad ensures that information is accessible to authorised users when needed?" },
                    { 2, "[\"Integrity\",\"Availability\",\"Confidentiality\",\"Non-repudiation\"]", 2, "Reading private data without authorisation violates Confidentiality — the property that prevents unauthorised disclosure of information.", 1, "A hacker intercepts network traffic and reads private emails without modifying them. Which CIA property has been violated?" },
                    { 3, "[\"Confidentiality\",\"Availability\",\"Integrity\",\"Accountability\"]", 1, "Ransomware primarily attacks Availability by making data inaccessible. Some attacks also impact Integrity by modifying files.", 1, "A ransomware attack encrypts files so the owner can no longer open them. Which CIA property is primarily affected?" },
                    { 4, "[\"sudo apt upgrade\",\"sudo apt update\",\"sudo apt install\",\"sudo apt refresh\"]", 1, "`sudo apt update` fetches the latest package information from repositories. It does not install or upgrade packages — that's `apt upgrade`.", 4, "Which command updates the package list on a Kali Linux system?" },
                    { 5, "[\"VirtualBox\",\"VMware\",\"WSL (Windows Subsystem for Linux)\",\"Docker Desktop\"]", 2, "WSL2 with a Kali Linux distribution from the Microsoft Store lets you run Kali tools directly inside Windows without a full VM overhead.", 4, "Which virtualisation option allows running Kali Linux on Windows without a full virtual machine?" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_UserId_CourseId",
                table: "Enrollments",
                columns: new[] { "UserId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseId",
                table: "Modules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_ModuleId",
                table: "QuizAttempts",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_UserId",
                table: "QuizAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_ModuleId",
                table: "QuizQuestions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
