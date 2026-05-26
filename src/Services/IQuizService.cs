// IQuizService.cs
using PwnLearn.Models;

namespace PwnLearn.Services;

/// <summary>Contract for quiz / self-assessment operations.</summary>
public interface IQuizService
{
    Task<List<QuizQuestion>> GetQuestionsForModuleAsync(int moduleId);
    Task<bool> SaveAttemptAsync(int userId, int moduleId, int score, int total);
    Task<List<QuizAttempt>> GetUserAttemptsAsync(int userId);
    Task<QuizAttempt?> GetBestAttemptAsync(int userId, int moduleId);

    // Admin CRUD
    Task<QuizQuestion?> GetQuestionByIdAsync(int id);
    Task<bool> CreateQuestionAsync(QuizQuestion question);
    Task<bool> UpdateQuestionAsync(QuizQuestion question);
    Task<bool> DeleteQuestionAsync(int id);
}
