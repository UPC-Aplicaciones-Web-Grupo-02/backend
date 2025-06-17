using System.Security.Cryptography;
using System.Text;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Users.Application.Internal.Service;
using backendmovix.Users.Domain.Model.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Users.Application.Internal.Service;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }

    public async Task<User> AuthenticateAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;
        if (!VerifyPassword(password, user.Password)) return null;
        return user;
    }
}