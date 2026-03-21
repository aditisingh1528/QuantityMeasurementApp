using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementRepository.Database
{
    public static class RepositoryFactory
    {
        public static IQuantityMeasurementRepository Create()
        {
            var config = DatabaseConfig.GetInstance();

            if (config.UseDatabase)
            {
                Console.WriteLine("[RepositoryFactory] Using Database Repository (SQL Server).");
                return QuantityMeasurementDatabaseRepository.GetInstance();
            }

            Console.WriteLine("[RepositoryFactory] Using Cache Repository (in-memory).");
            return QuantityMeasurementCacheRepository.GetInstance();
        }
    }
}
