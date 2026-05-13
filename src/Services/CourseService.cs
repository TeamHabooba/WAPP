// CourseService.cs
using Microsoft.EntityFrameworkCore;
using PwnLearn.Data;
using PwnLearn.Models;

namespace PwnLearn.Services;

/// <summary>Implements course catalogue and enrolment data operations.</summary>
public class CourseService : ICourseService
{
    private readonly AppDbContext _context;

    public CourseService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetAllPublishedAsync()
        => await _context.Courses
            .Where(c => c.IsPublished)
            .Include(c => c.Modules)
            .OrderBy(c => c.Id)
            .ToListAsync();

    public async Task<List<Course>> GetAllAsync()
        => await _context.Courses
            .Include(c => c.Modules)
            .OrderBy(c => c.Id)
            .ToListAsync();

    public async Task<Course?> GetByIdAsync(int id)
        => await _context.Courses.FindAsync(id);

    public async Task<Course?> GetWithModulesAsync(int id)
        => await _context.Courses
            .Include(c => c.Modules.OrderBy(m => m.OrderIndex))
            .ThenInclude(m => m.QuizQuestions)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> CreateAsync(Course course)
    {
        try
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        try
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var course = await _context.Courses.FindAsync(id);
            if (course is null) return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    // =====Enrolment

    public async Task<List<Enrollment>> GetUserEnrollmentsAsync(int userId)
        => await _context.Enrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.Course)
            .ThenInclude(c => c.Modules)
            .ToListAsync();

    public async Task<bool> EnrollAsync(int userId, int courseId)
    {
        if (await IsEnrolledAsync(userId, courseId)) return false;
        try
        {
            _context.Enrollments.Add(new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrolledAt = DateTime.UtcNow,
                CompletedModules = 0
            });
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> IsEnrolledAsync(int userId, int courseId)
        => await _context.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId);

    public async Task<bool> IncrementProgressAsync(int userId, int courseId)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        if (enrollment is null) return false;

        var totalModules = await _context.Modules.CountAsync(m => m.CourseId == courseId);
        if (enrollment.CompletedModules < totalModules)
        {
            enrollment.CompletedModules++;
            await _context.SaveChangesAsync();
        }
        return true;
    }
}
