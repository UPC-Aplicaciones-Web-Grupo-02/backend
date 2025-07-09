using backendmovix.Reservations.Applications.Internal.Service;
using backendmovix.Scooter.Application.Internal.Service;
using backendmovix.Shared.Application.Services;
using backendmovix.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendmovix.Shared.Infrastructure.Security;
using backendmovix.Suscriptions.Application.Internal.Service;
using backendmovix.Suscriptions.Domain.Model.Aggregate;
using backendmovix.Users.Application.Internal.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
// Comment
var builder = WebApplication.CreateBuilder(args);

// Define el nombre de la política CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // <-- Cambia a la URL de tu frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IScooterService, ScooterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ISuscriptionService, SuscriptionService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresa el token para loguearte.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userService = services.GetRequiredService<IUserService>();
    
// Inicializar la base de datos y agregar usuarios por defecto
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

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
