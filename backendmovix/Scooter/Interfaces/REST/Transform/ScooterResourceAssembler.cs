using backendmovix.Scooter.Interfaces.REST.Resources;

namespace backendmovix.Scooter.Interfaces.REST.Transform;

public static class ScooterResourceAssembler
{
    public static ScooterResource ToResource(Domain.Model.Aggregate.Scooter scooter)
    {
        return new ScooterResource
        {
            Id = scooter.Id,
            Name = scooter.Name,
            Description = scooter.Description,
            Image = scooter.Image,
            BrandId = scooter.BrandId,
            BrandName = scooter.Brand?.Name,         // Nuevo
            ModelId = scooter.ModelId,
            ModelName = scooter.Model?.Name,         // Nuevo
            DistrictId = scooter.DistrictId,
            DistrictName = scooter.District?.Name    // Nuevo
        };
    }
}