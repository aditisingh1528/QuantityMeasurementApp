using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementModel.Entities
{
    // UC17 Enhancement: QuantityMeasurementEntity with EF Core annotations
    [Table("QuantityMeasurements")]
    public class QuantityMeasurementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // first operand (this)
        [Required]
        [Column("this_value")]
        public double ThisValue { get; set; }

        [Required]
        [Column("this_unit")]
        public string ThisUnit { get; set; } = string.Empty;

        [Required]
        [Column("this_measurement_type")]
        public string ThisMeasurementType { get; set; } = string.Empty;

        // second operand (that)
        [Column("that_value")]
        public double ThatValue { get; set; }

        [Column("that_unit")]
        public string? ThatUnit { get; set; }

        [Column("that_measurement_type")]
        public string? ThatMeasurementType { get; set; }

        // result
        [Column("result_value")]
        public double ResultValue { get; set; }

        [Column("result_unit")]
        public string? ResultUnit { get; set; }

        [Column("result_measurement_type")]
        public string? ResultMeasurementType { get; set; }

        // operation type: COMPARE, CONVERT, ADD, SUBTRACT, MULTIPLY, DIVIDE
        [Required]
        [Column("operation")]
        public string Operation { get; set; } = string.Empty;

        // for comparison results
        [Column("result_string")]
        public string? ResultString { get; set; }

        // error tracking
        [Column("is_error")]
        public bool IsError { get; set; }

        [Column("error_message")]
        public string? ErrorMessage { get; set; }

        // audit timestamps – set on creation; UpdatedAt refreshed on every save
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        //  Constructors

        // EF Core requires a parameterless constructor
        public QuantityMeasurementEntity() { }

        // Two-operand operations: ADD, SUBTRACT, COMPARE, DIVIDE.
        public QuantityMeasurementEntity(
            double thisValue, string thisUnit, string thisMeasurementType,
            double thatValue, string thatUnit,
            string operation,
            double resultValue, string? resultUnit = null, string? resultString = null)
        {
            ThisValue           = thisValue;
            ThisUnit            = thisUnit;
            ThisMeasurementType = thisMeasurementType;
            ThatValue           = thatValue;
            ThatUnit            = thatUnit;
            ThatMeasurementType = thisMeasurementType;
            Operation           = operation;
            ResultValue         = resultValue;
            ResultUnit          = resultUnit;
            ResultString        = resultString;
            CreatedAt           = DateTime.UtcNow;
            UpdatedAt           = DateTime.UtcNow;
        }

        // Single-operand operation: CONVERT.
        public QuantityMeasurementEntity(
            double thisValue, string thisUnit, string thisMeasurementType,
            string operation,
            double resultValue, string resultUnit)
        {
            ThisValue           = thisValue;
            ThisUnit            = thisUnit;
            ThisMeasurementType = thisMeasurementType;
            Operation           = operation;
            ResultValue         = resultValue;
            ResultUnit          = resultUnit;
            CreatedAt           = DateTime.UtcNow;
            UpdatedAt           = DateTime.UtcNow;
        }

        // Error constructor – records the inputs and error message.
        public QuantityMeasurementEntity(
            double thisValue, string thisUnit,
            double thatValue, string? thatUnit,
            string operation, string errorMessage)
        {
            ThisValue    = thisValue;
            ThisUnit     = thisUnit;
            ThatValue    = thatValue;
            ThatUnit     = thatUnit;
            Operation    = operation;
            IsError      = true;
            ErrorMessage = errorMessage;
            CreatedAt    = DateTime.UtcNow;
            UpdatedAt    = DateTime.UtcNow;
        }

        // UC16 backward-compat properties so ConsoleApp and old service still compile
        [NotMapped] public double    Value               => ThisValue;
        [NotMapped] public string    Unit                => ThisUnit;
        [NotMapped] public string    MeasurementCategory => ThisMeasurementType;
        [NotMapped] public double    Operand1Value       => ThisValue;
        [NotMapped] public string    Operand1Unit        => ThisUnit;
        [NotMapped] public double?   Operand2Value       => ThatValue;
        [NotMapped] public string?   Operand2Unit        => ThatUnit;
        [NotMapped] public DateTime  Timestamp           => CreatedAt;

        public override string ToString()
        {
            return $"[{Id}] {Operation} | {ThisValue} {ThisUnit}" +
                   $" & {ThatValue} {ThatUnit}" +
                   $" => {ResultValue} {ResultUnit ?? ResultString}" +
                   $" | {CreatedAt:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
