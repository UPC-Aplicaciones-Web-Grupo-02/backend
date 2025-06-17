using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backendmovix.Suscriptions.Domain.Model.Aggregate;
using backendmovix.Users.Domain.Model.Aggregate;

namespace backendmovix.Reservations.Domain.Model.Aggregate;

public class Reservation
{
  [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string CantDate { get; set; }
    
    [Required]
    public int ScooterId { get; set; }
    public Scooter.Domain.Model.Aggregate.Scooter Scooter { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }
    
    [Required]
    public int SuscriptionId { get; set; }
    public Suscription Suscription { get; set; }  
}
