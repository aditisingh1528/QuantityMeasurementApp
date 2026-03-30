using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Models;
using QuantityMeasurementModel.Interfaces;

namespace QuantityMeasurementBusinessLayer.Mappers
{
    public static class QuantityMapper
    {
        // DTO → Model
        public static QuantityModel ToModel(QuantityDTO dto, IMeasurable converter)
        {
            return new QuantityModel(
                dto.Value,
                dto.Unit,
                converter.GetMeasurementType()
            );
        }

        // Model → DTO
        public static QuantityDTO ToDTO(QuantityModel model)
        {
            return new QuantityDTO(
                model.Value,
                model.Unit
            );
        }
    }
}