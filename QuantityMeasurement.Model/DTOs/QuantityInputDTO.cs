using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.DTOs
{
    /// <summary>Request DTO for compare, add, subtract, divide (two operands, no target unit).</summary>
    public class TwoOperandRequestDTO
    {
        [Required(ErrorMessage = "First quantity (this) is required")]
        public QuantityDTO? This { get; set; }

        [Required(ErrorMessage = "Second quantity (that) is required")]
        public QuantityDTO? That { get; set; }
    }

    /// <summary>Request DTO for convert (source value+unit, target unit only).</summary>
    public class ConvertRequestDTO
    {
        [Required(ErrorMessage = "Source quantity is required")]
        public QuantityDTO? From { get; set; }

        [Required(ErrorMessage = "Target unit is required")]
        public string? ToUnit { get; set; }
    }

    /// <summary>
    /// Request DTO for add-with-target-unit and subtract-with-target-unit.
    /// Caller provides two operands plus the unit they want the result expressed in.
    /// </summary>
    public class ArithmeticWithTargetRequestDTO
    {
        [Required(ErrorMessage = "First quantity (this) is required")]
        public QuantityDTO? This { get; set; }

        [Required(ErrorMessage = "Second quantity (that) is required")]
        public QuantityDTO? That { get; set; }

        [Required(ErrorMessage = "Target unit is required")]
        public string? TargetUnit { get; set; }
    }

    // ---- kept for backward-compat (tests / console app still use it) ----
    public class QuantityInputDTO
    {
        [Required(ErrorMessage = "First quantity cannot be null")]
        public QuantityDTO? ThisQuantityDTO { get; set; }

        [Required(ErrorMessage = "Second quantity cannot be null")]
        public QuantityDTO? ThatQuantityDTO { get; set; }

        public QuantityDTO? TargetQuantityDTO { get; set; }

        public QuantityInputDTO() { }
    }
}
