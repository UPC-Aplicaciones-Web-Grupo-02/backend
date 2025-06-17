namespace backend01.Scooter.Domain.Model.Aggregate;

public class Models
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Scooter> Scooters { get; set; }
}