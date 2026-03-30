using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementBusinessLayer.Interfaces
{
    public interface IQuantityMeasurementService
    {
        QuantityMeasurementDTO Compare(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO);

        QuantityMeasurementDTO Convert(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO);

        QuantityMeasurementDTO Add(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO);

        QuantityMeasurementDTO Add(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO,
                                   QuantityDTO targetUnitDTO);

        QuantityMeasurementDTO Subtract(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO);

        QuantityMeasurementDTO Subtract(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO,
                                        QuantityDTO targetUnitDTO);

        QuantityMeasurementDTO Divide(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO);

        List<QuantityMeasurementDTO> GetOperationHistory(string operation);

        List<QuantityMeasurementDTO> GetMeasurementsByType(string type);

        long GetOperationCount(string operation);

        List<QuantityMeasurementDTO> GetErrorHistory();

        bool AreLengthsEqual(Length l1, Length l2);
        bool AreWeightsEqual(Weight w1, Weight w2);
        List<QuantityMeasurementEntity> GetAllMeasurements();
        List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType);
        string GetPoolStatistics();
    }
}
