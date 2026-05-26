// Course.cs
using System.ComponentModel.DataAnnotations;

namespace PwnLearn.Models;

/// <summary>A cybersecurity learning course with multiple modules.</summary>
public class Course
{
    public int Id { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Category tag, e.g. "Fundamentals", "Tools", "CTF".</summary>
    [Required]
    [RegularExpression("^(Fundamentals|Tools|CTF)$")]
    public string Category { get; set; } = string.Empty;

    /// <summary>Difficulty: Beginner, Intermediate, Advanced.</summary>
    [Required]
    [RegularExpression("^(Beginner|Intermediate|Advanced)$")]
    public string Difficulty { get; set; } = "Beginner";

    /// <summary>Emoji or icon identifier for the card display.</summary>
    [StringLength(8)]
    public string IconEmoji { get; set; } = "🔐";

    public bool IsPublished { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
