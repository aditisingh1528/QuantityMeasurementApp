using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementModel.DTOs
{
    // Response DTO returned by every API endpoint.
    public class QuantityMeasurementDTO
    {
        //  inputs 
        public double  ThisValue           { get; set; }
        public string? ThisUnit            { get; set; }
        public string? ThisMeasurementType { get; set; }

        public double  ThatValue           { get; set; }
        public string? ThatUnit            { get; set; }
        public string? ThatMeasurementType { get; set; }

        //  operation 
        public string? Operation { get; set; }

        //  result 
        // Numeric result (conversion, arithmetic, divide ratio).
        public double  ResultValue           { get; set; }
        public string? ResultUnit            { get; set; }
        public string? ResultMeasurementType { get; set; }

        // Human-readable result for compare ("true" / "false").
        public string? ResultString { get; set; }

        //  error info 
        public bool    IsError       { get; set; }
        public string? ErrorMessage  { get; set; }

        public QuantityMeasurementDTO() { }

        public static QuantityMeasurementDTO FromEntity(QuantityMeasurementEntity entity) =>
            new()
            {
                ThisValue           = entity.ThisValue,
                ThisUnit            = entity.ThisUnit,
                ThisMeasurementType = entity.ThisMeasurementType,
                ThatValue           = entity.ThatValue,
                ThatUnit            = entity.ThatUnit,
                ThatMeasurementType = entity.ThatMeasurementType,
                Operation           = entity.Operation,
                ResultValue         = entity.ResultValue,
                ResultUnit          = entity.ResultUnit,
                ResultMeasurementType = entity.ResultMeasurementType,
                ResultString        = entity.ResultString,
                IsError             = entity.IsError,
                ErrorMessage        = entity.ErrorMessage
            };

        public QuantityMeasurementEntity ToEntity() =>
            new()
            {
                ThisValue           = ThisValue,
                ThisUnit            = ThisUnit            ?? string.Empty,
                ThisMeasurementType = ThisMeasurementType ?? string.Empty,
                ThatValue           = ThatValue,
                ThatUnit            = ThatUnit,
                ThatMeasurementType = ThatMeasurementType,
                Operation           = Operation           ?? string.Empty,
                ResultValue         = ResultValue,
                ResultUnit          = ResultUnit,
                ResultMeasurementType = ResultMeasurementType,
                ResultString        = ResultString,
                IsError             = IsError,
                ErrorMessage        = ErrorMessage,
                CreatedAt           = DateTime.UtcNow,
                UpdatedAt           = DateTime.UtcNow
            };

        public static List<QuantityMeasurementDTO> FromEntityList(
            List<QuantityMeasurementEntity> entities) =>
            entities.Select(FromEntity).ToList();

        public static List<QuantityMeasurementEntity> ToEntityList(
            List<QuantityMeasurementDTO> dtos) =>
            dtos.Select(d => d.ToEntity()).ToList();
    }
}
