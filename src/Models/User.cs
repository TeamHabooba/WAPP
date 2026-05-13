// User.cs
namespace PwnLearn.Models;

/// <summary>Represents a registered platform user.</summary>
public class User
{
    public int Id { get; set; }

    /// <summary>Display name shown on the platform.</summary>
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    /// <summary>BCrypt-hashed password — never stored in plaintext.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Role: "Member" or "Admin".</summary>
    public string Role { get; set; } = "Member";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
