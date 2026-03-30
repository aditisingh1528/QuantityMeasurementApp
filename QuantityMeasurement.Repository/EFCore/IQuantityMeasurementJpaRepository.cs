using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository.EFCore
{
    public interface IQuantityMeasurementJpaRepository
    {
        // basic CRUD
        void Save(QuantityMeasurementEntity entity);
        QuantityMeasurementEntity? FindById(int id);
        List<QuantityMeasurementEntity> FindAll();
        void Delete(QuantityMeasurementEntity entity);

        // Query methods

        // findByOperation
        List<QuantityMeasurementEntity> FindByOperation(string operation);

        // findByThisMeasurementType
        List<QuantityMeasurementEntity> FindByThisMeasurementType(string measurementType);

        // findByCreatedAtAfter(DateTime date)
        List<QuantityMeasurementEntity> FindByCreatedAtAfter(DateTime date);

        // findSuccessfulOperations(string operation) – custom query
        List<QuantityMeasurementEntity> FindSuccessfulOperations(string operation);

        // countByOperationAndIsErrorFalse(string operation)
        long CountByOperationAndIsErrorFalse(string operation);

        // findByIsErrorTrue()
        List<QuantityMeasurementEntity> FindByIsErrorTrue();
    }
}
