namespace QuantityMeasurementApp.Models
{
    // UC10-Common interface for all measurable units (Length, Weight, future units)
    public interface IMeasurable
    {
        double GetConversionFactor();

        double ConvertToBaseUnit(double value);

        double ConvertFromBaseUnit(double baseValue);

        string GetUnitName();
    }
}