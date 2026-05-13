// ICourseService.cs
using PwnLearn.Models;

namespace PwnLearn.Services;

/// <summary>Contract for course catalogue and enrolment operations.</summary>
public interface ICourseService
{
    Task<List<Course>> GetAllPublishedAsync();
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int id);
    Task<Course?> GetWithModulesAsync(int id);
    Task<bool> CreateAsync(Course course);
    Task<bool> UpdateAsync(Course course);
    Task<bool> DeleteAsync(int id);

    Task<List<Enrollment>> GetUserEnrollmentsAsync(int userId);
    Task<bool> EnrollAsync(int userId, int courseId);
    Task<bool> IsEnrolledAsync(int userId, int courseId);
    Task<bool> IncrementProgressAsync(int userId, int courseId);
}
