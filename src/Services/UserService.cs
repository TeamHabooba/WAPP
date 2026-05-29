// UserService.cs
using Microsoft.EntityFrameworkCore;

using PwnLearn.Data;


namespace PwnLearn.Services;


/// <summary>Implements user data operations using EF Core.</summary>
public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Models.User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLower().Trim());

    public async Task<Models.User?> GetByIdAsync(int id)
        => await _context.Users.FindAsync(id);

    public async Task<List<Models.User>> GetAllAsync()
        => await _context.Users.OrderBy(u => u.Id).ToListAsync();

    public async Task<bool> EmailExistsAsync(string email)
        => await _context.Users.AnyAsync(u => u.Email == email.ToLower().Trim());

    /// <summary>Validates credentials and returns the user, or null on failure.</summary>
    public async Task<Models.User?> AuthenticateAsync(string email, string password)
    {
        var user = await GetByEmailAsync(email);
        if (user is null || !user.IsActive) return null;
        // BCrypt verify never throws — returns false if hash does not match
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }

    /// <summary>Registers a new member; returns success flag and optional error message.</summary>
    public async Task<(bool Success, string? Error)> RegisterAsync(string name, string email, string password)
    {
        var normalised = email.ToLower().Trim();
        if (await EmailExistsAsync(normalised))
            return (false, "An account with this email already exists.");
        var user = new Models.User
        {
            Name = name.Trim(),
            Email = normalised,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Member",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return (true, null);
        }
        catch (DbUpdateException)
        {
            return (false, "Registration failed. Please try again.");
        }
    }

    public async Task<bool> UpdateAsync(Models.User user)
    {
        try
        {
            _context.Users.Update(user);
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
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> ToggleActiveAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return false;
        user.IsActive = !user.IsActive;
        return await UpdateAsync(user);
    }

    public async Task<(bool Success, string? Error)> AdminCreateAsync(
        string name, string email, string password, string role, bool isActive)
    {
        var normalisedEmail = email.ToLower().Trim();
        if (await EmailExistsAsync(normalisedEmail))
        {
            return (false, "An account with this email already exists.");
        }
        var user = new Models.User
        {
            Name = name.Trim(),
            Email = normalisedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), // Password hashig for extra sensitive data protection
            Role = role,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return (true, null);
        }
        catch (DbUpdateException)
        {
            return (false, "Failed to create user. Please try again.");
        }
    }
}
