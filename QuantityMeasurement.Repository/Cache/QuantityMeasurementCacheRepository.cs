using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementRepository.Cache
{
    // UC16 in-memory cache repository
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository? _instance;
        private readonly List<QuantityMeasurementEntity> _cache = new();

        private QuantityMeasurementCacheRepository() { }

        public static QuantityMeasurementCacheRepository GetInstance()
            => _instance ??= new QuantityMeasurementCacheRepository();

        public void SaveMeasurement(QuantityMeasurementEntity entity)
            => _cache.Add(entity);

        public List<QuantityMeasurementEntity> GetAllMeasurements()
            => _cache;

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
            => _cache.Where(e => e.Operation.Equals(operationType, StringComparison.OrdinalIgnoreCase)).ToList();

        public List<QuantityMeasurementEntity> GetMeasurementsByCategory(string category)
            => _cache.Where(e => e.MeasurementCategory.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

        public int GetTotalCount() => _cache.Count;

        public void DeleteAll() => _cache.Clear();

        public string GetPoolStatistics() => "Cache repository – no connection pool.";
    }
}
