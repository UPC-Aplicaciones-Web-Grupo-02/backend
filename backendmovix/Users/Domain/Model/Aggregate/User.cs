

namespace backendmovix.Users.Domain.Model.Aggregate;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Dni { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Photo { get; set; }
    public string Address { get; set; }
    public int RoleId { get; set; }
    public UserRole Role { get; set; }
}