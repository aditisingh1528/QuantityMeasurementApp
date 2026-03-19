namespace QuantityMeasurementModel.Entities
{
    public class QuantityMeasurementEntity
    {
        public double Value { get; }

        public string Unit { get; }

        public string Operation { get; }

        public QuantityMeasurementEntity(double value, string unit, string operation)
        {
            Value = value;
            Unit = unit;
            Operation = operation;
        }
    }
}