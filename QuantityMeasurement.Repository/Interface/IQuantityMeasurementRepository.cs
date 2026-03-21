using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository.Interface
{
    public interface IQuantityMeasurementRepository
    {
        void SaveMeasurement(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAllMeasurements();

        List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType);
        List<QuantityMeasurementEntity> GetMeasurementsByCategory(string category);
        int GetTotalCount();
        void DeleteAll();

        string GetPoolStatistics() => "Pool statistics not available for this repository.";
        void ReleaseResources() { /* no-op for cache repo */ }
    }
}
