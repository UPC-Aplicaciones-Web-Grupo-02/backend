using backendmovix.Users.Domain.Model.Aggregate;

namespace backendmovix.Shared.Application.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}