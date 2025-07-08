using System.Security.Cryptography;
using System.Text;
using backendmovix.Users.Domain.Model.Aggregate;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration; // para AppDbContext
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Users.Application.Internal.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var hashedPassword = HashPassword(password);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);

            return user; // si es null, credenciales inválidas
        }

        public async Task<User> RegisterAsync(string name, string email, string password)
        {
            var hashedPassword = HashPassword(password);

            var user = new User
            {
                Name = name,
                Email = email,
                Password = hashedPassword,
                // Puedes asignar otros valores por defecto si los necesitas
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}