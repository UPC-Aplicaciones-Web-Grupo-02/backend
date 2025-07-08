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
        var user = await _context.Users
            .Include(u => u.Role) // si usas navegación a tabla de roles
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) return null;

        if (!VerifyPassword(password, user.Password)) return null;

        return user;
    }

    public async Task<User> RegisterAsync(string name, string email, string password)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
            throw new InvalidOperationException("El correo ya está registrado.");

        var user = new User
        {
            Name = name,
            Email = email,
            Password = HashPassword(password),
            RoleId = 2 // puedes cambiarlo si tienes un rol predeterminado diferente
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}