using backendmovix.Scooter.Domain.Model.Aggregate;
using backendmovix.Reservations.Domain.Model.Aggregate;
using backendmovix.Scooter.Domain.Model.Aggregate;
using backendmovix.Suscriptions.Domain.Model.Aggregate;
using backendmovix.Users.Domain.Model.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Scooter.Domain.Model.Aggregate.Scooter> Scooters { get; set; }
    public DbSet<Brands> Brands { get; set; }
    public DbSet<Models> Models { get; set; }
    public DbSet<Districts> Districts { get; set; }
    
    public DbSet<User> Users { get; set; } // <-- Agregado
    
    public DbSet<UserRole> UserRoles { get; set; } // <-- Agregado
    
    public DbSet<Suscription> Suscriptions { get; set; }
    public DbSet<TypeSuscription> TypeSuscriptions { get; set; }
    
    public DbSet<Reservation> Reservations { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Scooter.Domain.Model.Aggregate.Scooter>().HasKey(a => a.Id);
        builder.Entity<Scooter.Domain.Model.Aggregate.Scooter>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Scooter.Domain.Model.Aggregate.Scooter>().Property(a => a.Name).IsRequired().HasMaxLength(50);
        builder.Entity<Scooter.Domain.Model.Aggregate.Scooter>().Property(a => a.Description).IsRequired().HasMaxLength(240);
        
        builder.Entity<Scooter.Domain.Model.Aggregate.Scooter>()
            .HasOne(s => s.Brand)
            .WithMany(b => b.Scooters)
            .HasForeignKey(s => s.BrandId);

        builder.Entity<Scooter.Domain.Model.Aggregate.Scooter>()
            .HasOne(s => s.Model)
            .WithMany(m => m.Scooters)
            .HasForeignKey(s => s.ModelId);

        builder.Entity<Scooter.Domain.Model.Aggregate.Scooter>()
            .HasOne(s => s.District)
            .WithMany(d => d.Scooters)
            .HasForeignKey(s => s.DistrictId);

        // Datos fijos
        builder.Entity<Brands>().HasData(
            new Brands { Id = 1, Name = "Xiaomi" },
            new Brands { Id = 2, Name = "Segway" }
        );
        builder.Entity<Models>().HasData(
            new Models { Id = 1, Name = "M365" },
            new Models { Id = 2, Name = "Ninebot" }
        );
        builder.Entity<Districts>().HasData(
            new Districts { Id = 1, Name = "Miraflores" }
        );
        
        // Configuración UserRole ------------------------------------------------------------------
        builder.Entity<UserRole>().HasKey(r => r.Id);
        builder.Entity<UserRole>().Property(r => r.Role).IsRequired().HasMaxLength(50);

        // Configuración User
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(100);
        builder.Entity<User>().Property(u => u.Password).IsRequired();
        builder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        // Datos fijos para UserRole
        builder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, Role = "Administrador" },
            new UserRole { Id = 2, Role = "Usuario" },
            new UserRole { Id = 3, Role = "Universitario" }
        );
        
        // Configuración TypeSuscription
        builder.Entity<TypeSuscription>().HasKey(t => t.Id);
        builder.Entity<TypeSuscription>().Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Entity<TypeSuscription>().Property(t => t.Costo).IsRequired();

        // Configuración Suscription
        builder.Entity<Suscription>().HasKey(s => s.Id);
        builder.Entity<Suscription>().Property(s => s.Number).IsRequired();
        builder.Entity<Suscription>().Property(s => s.Date).IsRequired();
        builder.Entity<Suscription>().Property(s => s.Cvv).IsRequired();
        builder.Entity<Suscription>()
            .HasOne(s => s.Type)
            .WithMany(t => t.Suscriptions)
            .HasForeignKey(s => s.TypeId);
        
        // Configuración Reservation
        builder.Entity<Reservation>().HasKey(r => r.Id);
        builder.Entity<Reservation>().Property(r => r.CantDate).IsRequired();
        builder.Entity<Reservation>()
            .HasOne(r => r.Scooter)
            .WithMany()
            .HasForeignKey(r => r.ScooterId);
        builder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
        builder.Entity<Reservation>()
            .HasOne(r => r.Suscription)
            .WithMany()
            .HasForeignKey(r => r.SuscriptionId);
    }
}