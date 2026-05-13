// Module.cs
namespace PwnLearn.Models;

/// <summary>A single lesson module inside a course.</summary>
public class Module
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    /// <summary>Display order within the parent course.</summary>
    public int OrderIndex { get; set; }

    // Navigation
    public Course Course { get; set; } = default!;
    public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
