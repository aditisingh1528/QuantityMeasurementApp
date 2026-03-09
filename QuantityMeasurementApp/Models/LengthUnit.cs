using System;

namespace QuantityMeasurementApp.Models
{
    public enum LengthUnit
    {
        FEET = 1,
        INCHES = 2,
        YARDS = 3,
        CENTIMETERS = 4
    }

    public static class LengthUnitExtensions
    {
        // Conversion factor TO FEET (Base Unit)
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => 1.0,
                LengthUnit.INCHES => 1.0 / 12.0,
                LengthUnit.YARDS => 3.0,
                LengthUnit.CENTIMETERS => 1.0 / 30.48,
                _ => throw new ArgumentException("Invalid unit.")
            };
        }

        // Convert value IN THIS UNIT to FEET (Base Unit)
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            double result = value * unit.GetConversionFactor();
            return Math.Round(result, 6);
        }

        // Convert value FROM FEET (Base Unit) to THIS UNIT
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            double result = baseValue / unit.GetConversionFactor();
            return Math.Round(result, 6);
        }
    }
}