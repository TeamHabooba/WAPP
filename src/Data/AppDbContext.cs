// AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using PwnLearn.Models;

namespace PwnLearn.Data;

/// <summary>Entity Framework Core database context for PwnLearn.</summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<QuizAttempt> QuizAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.UserId, e.CourseId })
            .IsUnique();

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Admin", Email = "admin@pwnlearn.io",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@1234"),
                Role = "Admin", IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 2, Name = "Demo Member", Email = "demo@pwnlearn.io",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Member@1234"),
                Role = "Member", IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Title = "Cybersecurity Fundamentals",
                Description = "Core concepts of information security: CIA triad, threat landscape, attack surfaces, and defence strategies.",
                Category = "Fundamentals", Difficulty = "Beginner", IconEmoji = "🛡️", IsPublished = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Course { Id = 2, Title = "Kali Linux Essentials",
                Description = "Get comfortable with Kali Linux — the industry-standard penetration-testing distribution.",
                Category = "Tools", Difficulty = "Beginner", IconEmoji = "🐉", IsPublished = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Course { Id = 3, Title = "Metasploit Framework",
                Description = "Learn to use Metasploit for vulnerability assessment and controlled exploitation in lab environments.",
                Category = "Tools", Difficulty = "Intermediate", IconEmoji = "⚡", IsPublished = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Course { Id = 4, Title = "CTF Challenges & Strategy",
                Description = "Introduction to Capture-the-Flag competitions: web exploitation, reverse engineering, and cryptography.",
                Category = "CTF", Difficulty = "Intermediate", IconEmoji = "🚩", IsPublished = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Course { Id = 5, Title = "Ethical Hacking & Responsible Disclosure",
                Description = "Understand the legal and ethical framework around penetration testing and vulnerability reporting.",
                Category = "Fundamentals", Difficulty = "Beginner", IconEmoji = "⚖️", IsPublished = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Course { Id = 6, Title = "Network Reconnaissance",
                Description = "Master Nmap, Wireshark, and passive OSINT techniques for network discovery.",
                Category = "Tools", Difficulty = "Advanced", IconEmoji = "🔍", IsPublished = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Module>().HasData(
            new Module { Id = 1, CourseId = 1, OrderIndex = 1, Title = "The CIA Triad",
                Content = "Confidentiality, Integrity, and Availability form the cornerstone of information security. Every security control can be mapped back to protecting one or more of these three properties. Confidentiality prevents unauthorised disclosure; Integrity ensures data is not tampered with; Availability guarantees that systems and data are accessible when needed." },
            new Module { Id = 2, CourseId = 1, OrderIndex = 2, Title = "Threat Actors & Motivations",
                Content = "Understanding who attacks systems and why is essential for building effective defences. Threat actors range from script kiddies and hacktivists to nation-state APT groups. Motivations include financial gain, espionage, ideological causes, and notoriety. Mapping actors to the MITRE ATT&CK framework helps defenders anticipate tactics." },
            new Module { Id = 3, CourseId = 1, OrderIndex = 3, Title = "Attack Surfaces & Vectors",
                Content = "An attack surface is the sum of all points where an attacker could try to enter or extract data from a system. Common vectors include phishing, unpatched software, weak credentials, and misconfigured cloud resources. Reducing the attack surface through least privilege, patch management, and network segmentation is a primary defensive strategy." },
            new Module { Id = 4, CourseId = 2, OrderIndex = 1, Title = "Installing & Configuring Kali",
                Content = "Kali Linux can be installed as a VM (VirtualBox/VMware), run live from USB, or deployed in WSL on Windows. After installation, update the package list with `sudo apt update && sudo apt upgrade`. The default toolset includes over 600 security tools pre-configured and ready to use." },
            new Module { Id = 5, CourseId = 2, OrderIndex = 2, Title = "Essential Terminal Commands",
                Content = "Mastery of the Linux terminal is non-negotiable for penetration testers. Key commands: `ls`, `cd`, `cat`, `grep`, `find`, `chmod`, `netstat`, `ps`, and `kill`. Piping (`|`) and redirection (`>`, `>>`) allow powerful one-liners. Practice building compound commands to automate reconnaissance tasks." },
            new Module { Id = 6, CourseId = 3, OrderIndex = 1, Title = "Metasploit Architecture",
                Content = "Metasploit Framework consists of modules: Exploits, Payloads, Auxiliaries, Encoders, and Post-exploitation modules. The `msfconsole` is the primary interface. Understanding the difference between a bind shell and a reverse shell payload is fundamental before running any module." },
            new Module { Id = 7, CourseId = 3, OrderIndex = 2, Title = "Your First Exploit (Lab)",
                Content = "Using a controlled Metasploitable VM, we practice the full exploitation cycle: `search`, `use`, `set RHOSTS`, `set PAYLOAD`, `run`. Always confirm you have written permission before testing any system. Document your findings in a structured report with steps to reproduce and suggested remediation." }
        );

        modelBuilder.Entity<QuizQuestion>().HasData(
            new QuizQuestion { Id = 1, ModuleId = 1,
                QuestionText = "Which element of the CIA triad ensures that information is accessible to authorised users when needed?",
                ChoicesJson = "[\"Confidentiality\",\"Integrity\",\"Availability\",\"Authentication\"]",
                CorrectIndex = 2,
                Explanation = "Availability guarantees that systems and data remain accessible to authorised users. DDoS attacks typically target availability." },
            new QuizQuestion { Id = 2, ModuleId = 1,
                QuestionText = "A hacker intercepts network traffic and reads private emails without modifying them. Which CIA property has been violated?",
                ChoicesJson = "[\"Integrity\",\"Availability\",\"Confidentiality\",\"Non-repudiation\"]",
                CorrectIndex = 2,
                Explanation = "Reading private data without authorisation violates Confidentiality — the property that prevents unauthorised disclosure of information." },
            new QuizQuestion { Id = 3, ModuleId = 1,
                QuestionText = "A ransomware attack encrypts files so the owner can no longer open them. Which CIA property is primarily affected?",
                ChoicesJson = "[\"Confidentiality\",\"Availability\",\"Integrity\",\"Accountability\"]",
                CorrectIndex = 1,
                Explanation = "Ransomware primarily attacks Availability by making data inaccessible. Some attacks also impact Integrity by modifying files." },
            new QuizQuestion { Id = 4, ModuleId = 4,
                QuestionText = "Which command updates the package list on a Kali Linux system?",
                ChoicesJson = "[\"sudo apt upgrade\",\"sudo apt update\",\"sudo apt install\",\"sudo apt refresh\"]",
                CorrectIndex = 1,
                Explanation = "`sudo apt update` fetches the latest package information from repositories. It does not install or upgrade packages — that's `apt upgrade`." },
            new QuizQuestion { Id = 5, ModuleId = 4,
                QuestionText = "Which virtualisation option allows running Kali Linux on Windows without a full virtual machine?",
                ChoicesJson = "[\"VirtualBox\",\"VMware\",\"WSL (Windows Subsystem for Linux)\",\"Docker Desktop\"]",
                CorrectIndex = 2,
                Explanation = "WSL2 with a Kali Linux distribution from the Microsoft Store lets you run Kali tools directly inside Windows without a full VM overhead." }
        );

        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment { Id = 1, UserId = 2, CourseId = 1,
                EnrolledAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                CompletedModules = 1 }
        );
    }
}
