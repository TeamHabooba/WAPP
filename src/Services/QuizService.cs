// QuizService.cs
using Microsoft.EntityFrameworkCore;
using PwnLearn.Data;
using PwnLearn.Models;

namespace PwnLearn.Services;

/// <summary>Handles quiz data retrieval and attempt recording.</summary>
public class QuizService : IQuizService
{
    private readonly AppDbContext _context;

    public QuizService(AppDbContext context)
    {
        _context = context;
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
}
