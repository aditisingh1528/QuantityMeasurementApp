namespace QuantityMeasurementApp.Models
{
    // Functional interface to check arithmetic support
    public delegate bool SupportsArithmetic();

    public interface IMeasurable
    {
        double GetConversionFactor();

        double ConvertToBaseUnit(double value);

        double ConvertFromBaseUnit(double baseValue);

        string GetUnitName();

        // Default lambda: arithmetic supported
        public static SupportsArithmetic supportsArithmetic = () => true;

        // Default method
        public virtual bool SupportsArithmeticOperation()
        {
            return supportsArithmetic();
        }

        // Validate operation support
        public virtual void ValidateOperationSupport(string operation)
        {
            
        }
    }
}