// IModuleService.cs
using PwnLearn.Models;


namespace PwnLearn.Services;


public interface IModuleService
{
    Task<List<Module>> GetByCourseAsync(int courseId);
    Task<Module?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Module module);
    Task<bool> UpdateAsync(Module module);
    Task<bool> DeleteAsync(int id);
}
