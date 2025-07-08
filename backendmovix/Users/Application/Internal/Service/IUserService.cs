using backendmovix.Users.Domain.Model.Aggregate;

namespace backendmovix.Users.Application.Internal.Service;

public interface IUserService
{
    Task<User> AuthenticateAsync(string email, string password);
    Task<User> RegisterAsync(string name, string email, string password); 
    string HashPassword(string password);
}