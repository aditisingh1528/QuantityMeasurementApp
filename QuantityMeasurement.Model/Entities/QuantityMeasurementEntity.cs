namespace QuantityMeasurementModel.Entities
{
    /// <summary>
    /// Represents a persisted record of a quantity measurement operation.
    /// Extended in UC16 to carry all columns required by the database table.
    /// </summary>
    public class QuantityMeasurementEntity
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public string Operation { get; set; }

        public int Id { get; set; }

        /// <summary>Category: Length | Weight | Volume | Temperature</summary>
        public string MeasurementCategory { get; set; }

        public double Operand1Value { get; set; }
        public string Operand1Unit { get; set; }

        public double? Operand2Value { get; set; }
        public string? Operand2Unit { get; set; }

        public double? ResultValue { get; set; }
        public string? ResultUnit { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime Timestamp { get; set; }

        public QuantityMeasurementEntity(double value, string unit, string operation)
        {
            Value = value;
            Unit = unit;
            Operation = operation;
            MeasurementCategory = string.Empty;
            Operand1Value = value;
            Operand1Unit = unit;
            Timestamp = DateTime.Now;
        }

        //UC16 full constructor
        public QuantityMeasurementEntity(
            string operationType,
            string measurementCategory,
            double operand1Value,
            string operand1Unit,
            double? operand2Value = null,
            string? operand2Unit = null,
            double? resultValue = null,
            string? resultUnit = null,
            string? errorMessage = null)
        {
            Operation = operationType;
            MeasurementCategory = measurementCategory;
            Operand1Value = operand1Value;
            Operand1Unit = operand1Unit;
            Operand2Value = operand2Value;
            Operand2Unit = operand2Unit;
            ResultValue = resultValue;
            ResultUnit = resultUnit;
            ErrorMessage = errorMessage;
            Timestamp = DateTime.Now;

            Value = operand1Value;
            Unit = operand1Unit;
        }

        public override string ToString()
        {
            return $"[{Id}] {Operation} | {Operand1Value} {Operand1Unit}" +
                   $"{(Operand2Value.HasValue ? $" & {Operand2Value} {Operand2Unit}" : "")}" +
                   $" => {ResultValue} {ResultUnit} | {Timestamp:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
