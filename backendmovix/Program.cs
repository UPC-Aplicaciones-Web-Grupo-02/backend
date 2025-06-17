using backendmovix.Reservations.Applications.Internal.Service;
using backendmovix.Scooter.Application.Internal.Service;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Suscriptions.Application.Internal.Service;
using backendmovix.Suscriptions.Domain.Model.Aggregate;
using backendmovix.Users.Application.Internal.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();    
builder.Services.AddSwaggerGen();              
builder.Services.AddScoped<IScooterService, ScooterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISuscriptionService, SuscriptionService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())  
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userService = services.GetRequiredService<IUserService>();

    context.Database.EnsureCreated();
    if (!context.TypeSuscriptions.Any())
    {
        context.TypeSuscriptions.AddRange(
            new TypeSuscription { Name = "Plan Semanal", Costo = "29.90" },
            new TypeSuscription { Name = "Plan Mensual", Costo = "79.90" },
            new TypeSuscription { Name = "Plan Trimestral", Costo = "109.90" }
        );
        context.SaveChanges();
    }
}
app.UseSwagger(); 
app.UseSwaggerUI(); 
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(builder =>
    builder.WithOrigins("http://localhost:5184")
        .AllowAnyHeader()
        .AllowAnyMethod());
app.UseAuthorization();
app.MapControllers();
app.Run();