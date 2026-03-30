using Microsoft.EntityFrameworkCore;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementRepository.EFCore;
using QuantityMeasurementWebApi.Config;
using QuantityMeasurementWebApi.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var useInMemory = string.IsNullOrWhiteSpace(connectionString) ||
                  connectionString.Equals("InMemory", StringComparison.OrdinalIgnoreCase);

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
{
    if (useInMemory)
        options.UseInMemoryDatabase("QuantityMeasurementDb");
    else
        options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IQuantityMeasurementJpaRepository,
                            QuantityMeasurementJpaRepository>();

builder.Services.AddScoped<IQuantityMeasurementService,
                            QuantityMeasurementServiceImpl>();

builder.Services.AddScoped<IAuthService, AuthServiceImpl>();

builder.Services.AddSecurityConfig(builder.Configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionHandler>();
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Quantity Measurement API",
        Version     = "1.0.0",
        Description = "REST API for quantity measurements with support for multiple unit types"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your valid token in the text input below."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
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
    var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    if (useInMemory)
        db.Database.EnsureCreated();
    else
        db.Database.Migrate();
}

var env    = app.Environment.EnvironmentName;
var dbMode = useInMemory ? "In-Memory" : "SQL Server";

Console.WriteLine("Quantity Measurement API started");
Console.WriteLine($"  Environment : {env}");
Console.WriteLine($"  Database    : {dbMode}");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
    c.RoutePrefix = "swagger";  // accessible at /swagger/index.html
});
Console.WriteLine("  Swagger UI  : http://localhost:8080/swagger");

app.UseSecurityConfig();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
