using System.ComponentModel.DataAnnotations;

namespace backendmovix.Users.Interfaces.REST.Resources;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}