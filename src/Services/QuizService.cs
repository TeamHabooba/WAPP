// QuizService.cs
using Microsoft.EntityFrameworkCore;
using PwnLearn.Data;
using PwnLearn.Models;

namespace PwnLearn.Services;

/// <summary>Handles quiz data retrieval and attempt recording.</summary>
public class QuizService : IQuizService
{
    private readonly AppDbContext _context;
    private readonly ILogger<QuizService> _logger;

    public QuizService(AppDbContext context, ILogger<QuizService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<QuizQuestion>> GetQuestionsForModuleAsync(int moduleId)
        => await _context.QuizQuestions
            .Where(q => q.ModuleId == moduleId)
            .ToListAsync();

    public async Task<bool> SaveAttemptAsync(int userId, int moduleId, int score, int total)
    {
        try
        {
            _context.QuizAttempts.Add(new QuizAttempt
            {
                UserId = userId,
                ModuleId = moduleId,
                Score = score,
                TotalQuestions = total,
                AttemptedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<List<QuizAttempt>> GetUserAttemptsAsync(int userId)
        => await _context.QuizAttempts
            .Where(a => a.UserId == userId)
            .Include(a => a.Module)
            .OrderByDescending(a => a.AttemptedAt)
            .ToListAsync();

    public async Task<QuizAttempt?> GetBestAttemptAsync(int userId, int moduleId)
        => await _context.QuizAttempts
            .Where(a => a.UserId == userId && a.ModuleId == moduleId)
            .OrderByDescending(a => a.Score)
            .FirstOrDefaultAsync();

    // =====Admin CRUD

    public async Task<QuizQuestion?> GetQuestionByIdAsync(int id)
        => await _context.QuizQuestions.FindAsync(id);

    public async Task<bool> CreateQuestionAsync(QuizQuestion question)
    {
        try
        {
            _context.QuizQuestions.Add(question);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create question for module {ModuleId}.", question.ModuleId);
            return false;
        }
    }

    public async Task<bool> UpdateQuestionAsync(QuizQuestion question)
    {
        try
        {
            _context.QuizQuestions.Update(question);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update question {QuestionId}.", question.Id);
            return false;
        }
    }

    public async Task<bool> DeleteQuestionAsync(int id)
    {
        try
        {
            var q = await _context.QuizQuestions.FindAsync(id);
            if (q is null) return false;
            _context.QuizQuestions.Remove(q);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete question {QuestionId}.", id);
            return false;
        }
    }
}
