// QuizAttempt.cs
namespace PwnLearn.Models;

/// <summary>Records the result of a member's quiz attempt on a module.</summary>
public class QuizAttempt
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ModuleId { get; set; }

    public int Score { get; set; }

    public int TotalQuestions { get; set; }

    public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = default!;
    public Module Module { get; set; } = default!;
}
