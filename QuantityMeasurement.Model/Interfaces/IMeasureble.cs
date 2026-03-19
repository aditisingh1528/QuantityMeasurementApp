namespace QuantityMeasurementModel.Interfaces
{
    public interface IMeasurable
    {
        double ConvertToBase(double value);
        double ConvertFromBase(double baseValue);
        string GetMeasurementType();
    }
}