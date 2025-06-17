namespace backend01.Scooter.Interfaces.REST.Resources;

public class CreateScooterResource
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int BrandId { get; set; }
    public int ModelId { get; set; }
    public int DistrictId { get; set; }
}