using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.DTOs
{
    public class QuantityDTO
    {
        [Required(ErrorMessage = "Value cannot be empty")]
        public double Value { get; set; }

        [Required(ErrorMessage = "Unit cannot be null")]
        public string Unit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Measurement type cannot be null")]
        [RegularExpression(
            "^(LengthUnit|VolumeUnit|WeightUnit|TemperatureUnit)$",
            ErrorMessage = "Measurement type must be one of: LengthUnit, VolumeUnit, WeightUnit, TemperatureUnit")]
        public string MeasurementType { get; set; } = string.Empty;

        public QuantityDTO() { }

        public QuantityDTO(double value, string unit, string measurementType = "")
        {
            Value           = value;
            Unit            = unit;
            MeasurementType = measurementType;
        }
        public QuantityDTO(double value, string unit)
        {
            Value = value;
            Unit  = unit;
        }
    }
}
