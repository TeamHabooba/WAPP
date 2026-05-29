// IUserService.cs
using PwnLearn.Models;


namespace PwnLearn.Services;


/// <summary>Contract for user-related data operations.</summary>
public interface IUserService
{
    Task<Models.User?> GetByEmailAsync(string email);
    Task<Models.User?> GetByIdAsync(int id);
    Task<List<Models.User>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);

    Task<Models.User?> AuthenticateAsync(string email, string password);

    Task<(bool Success, string? Error)> RegisterAsync(string name, string email, string password);
    Task<bool> UpdateAsync(Models.User user);
    Task<bool> DeleteAsync(int id);
    Task<bool> ToggleActiveAsync(int id);
    Task<(bool Success, string? Error)> AdminCreateAsync(string name, string email, string password, string role, bool isActive);
}
