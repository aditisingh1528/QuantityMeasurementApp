using System;

namespace QuantityMeasurementApp.Models
{
    public enum LengthUnit
    {
        FEET = 12,
        INCHES = 1,
        YARDS = 36,
        CENTIMETERS = 0
    }

    public static class LengthUnitExtensions
    {
        public static double GetConversionFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => 12.0,
                LengthUnit.INCHES => 1.0,
                LengthUnit.YARDS => 36.0,
                LengthUnit.CENTIMETERS => 0.393701,
                _ => throw new ArgumentException("Invalid unit")
            };
        }

        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return Math.Round(baseValue / unit.GetConversionFactor(), 6);
        }

        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString();
        }
    }
}