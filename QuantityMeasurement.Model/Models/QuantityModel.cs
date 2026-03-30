namespace QuantityMeasurementModel.Models
{
    public class QuantityModel
    {
        public double Value { get; set; }

        public string Unit { get; set; }

        public string MeasurementType { get; set; }

        public QuantityModel(double value, string unit, string type)
        {
            Value = value;
            Unit = unit;
            MeasurementType = type;
        }
    }
}