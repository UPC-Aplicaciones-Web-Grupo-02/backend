namespace backendmovix.Scooter.Interfaces.REST.Resources;

public class ScooterResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; }      // Nuevo
    public int ModelId { get; set; }
    public string ModelName { get; set; }      // Nuevo
    public int DistrictId { get; set; }
    public string DistrictName { get; set; }   // Nuevo
}