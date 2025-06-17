namespace backendmovix.Scooter.Domain.Model.Aggregate;

public class Scooter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int BrandId { get; set; }
    public Brands Brand { get; set; }
    public int ModelId { get; set; }
    public Models Model { get; set; }
    public int DistrictId { get; set; }
    public Districts District { get; set; }
    
}