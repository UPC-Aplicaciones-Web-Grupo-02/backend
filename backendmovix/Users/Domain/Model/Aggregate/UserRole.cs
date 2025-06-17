namespace backendmovix.Users.Domain.Model.Aggregate;

public class UserRole
{
    public int Id { get; set; }
    public string Role { get; set; }
    public ICollection<User> Users { get; set; }
}