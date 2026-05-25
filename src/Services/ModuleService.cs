// ModuleService.cs
using Microsoft.EntityFrameworkCore;
using PwnLearn.Data;
using PwnLearn.Models;

namespace PwnLearn.Services;

public class ModuleService : IModuleService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ModuleService> _logger;

    public ModuleService(AppDbContext context, ILogger<ModuleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Module>> GetByCourseAsync(int courseId)
        => await _context.Modules
            .Where(m => m.CourseId == courseId)
            .Include(m => m.QuizQuestions)
            .OrderBy(m => m.OrderIndex)
            .ToListAsync();

    public async Task<Module?> GetByIdAsync(int id)
        => await _context.Modules.FindAsync(id);

    public async Task<bool> CreateAsync(Module module)
    {
        try
        {
            var maxOrder = await _context.Modules
                .Where(m => m.CourseId == module.CourseId)
                .Select(m => (int?)m.OrderIndex)
                .MaxAsync() ?? 0;
            module.OrderIndex = maxOrder + 1;

            _context.Modules.Add(module);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create module for course {CourseId}.", module.CourseId);
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Module module)
    {
        try
        {
            _context.Modules.Update(module);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update module {ModuleId}.", module.Id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var module = await _context.Modules.FindAsync(id);
            if (module is null) return false;
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete module {ModuleId}.", id);
            return false;
        }
    }
}
