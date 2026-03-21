using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementRepository.Cache
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository? _instance;

        private readonly List<QuantityMeasurementEntity> _cache = new();

        private QuantityMeasurementCacheRepository() { }

        public static QuantityMeasurementCacheRepository GetInstance()
        {
            return _instance ??= new QuantityMeasurementCacheRepository();
        }

        public void SaveMeasurement(QuantityMeasurementEntity entity)
        {
            _cache.Add(entity);
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return _cache;
        }

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
        {
            return _cache
                .Where(e => e.Operation.Equals(operationType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<QuantityMeasurementEntity> GetMeasurementsByCategory(string category)
        {
            return _cache
                .Where(e => e.MeasurementCategory.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public int GetTotalCount() => _cache.Count;

        public void DeleteAll() => _cache.Clear();

        public string GetPoolStatistics() => "Cache repository – no connection pool.";
    }
}
