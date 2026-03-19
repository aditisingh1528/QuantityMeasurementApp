using QuantityMeasurementModel.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementRepository.Interface
{
    public interface IQuantityMeasurementRepository
    {
        void SaveMeasurement(QuantityMeasurementEntity entity);

        List<QuantityMeasurementEntity> GetAllMeasurements();
    }
}