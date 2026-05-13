// QuizQuestion.cs
namespace PwnLearn.Models;

/// <summary>A multiple-choice question attached to a module.</summary>
public class QuizQuestion
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    /// <summary>JSON-serialised string array of 4 choices.</summary>
    public string ChoicesJson { get; set; } = "[]";

    /// <summary>0-based index of the correct answer in Choices.</summary>
    public int CorrectIndex { get; set; }

    public string Explanation { get; set; } = string.Empty;

    // Navigation
    public Module Module { get; set; } = default!;
}
