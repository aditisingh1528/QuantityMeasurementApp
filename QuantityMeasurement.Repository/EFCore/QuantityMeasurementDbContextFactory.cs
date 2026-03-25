using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementRepository.EFCore
{
    /// <summary>
    /// Lets "dotnet ef migrations add / database update" find the DbContext
    /// without needing to boot the full WebApi host. Reads the connection string
    /// from WebApi/appsettings.Production.json so it targets your real SQL Server.
    /// </summary>
    public class QuantityMeasurementDbContextFactory
        : IDesignTimeDbContextFactory<QuantityMeasurementDbContext>
    {
        public QuantityMeasurementDbContext CreateDbContext(string[] args)
        {
            // Walk up from the Repository project to find the WebApi appsettings
            var basePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..", "QuantityMeasurement.WebApi");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Production.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<QuantityMeasurementDbContext>();

            if (string.IsNullOrWhiteSpace(connectionString) ||
                connectionString.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
            {
                // Fallback: use LocalDB so dotnet ef can still generate SQL scripts
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=QuantityMeasurementDB;Trusted_Connection=True;");
            }
            else
            {
                optionsBuilder.UseSqlServer(connectionString);
            }

            return new QuantityMeasurementDbContext(optionsBuilder.Options);
        }
    }
}
