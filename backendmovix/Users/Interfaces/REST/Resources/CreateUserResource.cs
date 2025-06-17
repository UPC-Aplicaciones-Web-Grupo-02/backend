using System.ComponentModel.DataAnnotations;

namespace backendmovix.Users.Interfaces.REST.Resources;

public class CreateUserResource
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Dni { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string Photo { get; set; }
    public string Address { get; set; }
    [Required]
    public int RoleId { get; set; }
}