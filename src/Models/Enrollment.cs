// Enrollment.cs
namespace PwnLearn.Models;

/// <summary>Records that a member is enrolled in a course.</summary>
public class Enrollment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    /// <summary>Number of modules the member has completed in this course.</summary>
    public int CompletedModules { get; set; } = 0;

    // Navigation
    public User User { get; set; } = default!;
    public Course Course { get; set; } = default!;
}
