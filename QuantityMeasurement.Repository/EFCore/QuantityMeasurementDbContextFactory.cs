using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace QuantityMeasurementRepository.EFCore
{
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

            var resolvedConnection = (string.IsNullOrWhiteSpace(connectionString) || connectionString.Equals("InMemory", StringComparison.OrdinalIgnoreCase)) ?       "Host=localhost;Database=QuantityMeasurementDB;Username=postgres;         Password=postgres" : connectionString;

            optionsBuilder.UseNpgsql(resolvedConnection);

            return new QuantityMeasurementDbContext(optionsBuilder.Options);
        }
    }
}
