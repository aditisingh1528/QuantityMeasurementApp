using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents supported weight units.
    /// Base unit: Kilogram
    /// </summary>
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        private const double Epsilon = 1e-6;

        /// <summary>
        /// Returns conversion factor relative to base unit (Kilogram).
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => 1.0,
                WeightUnit.GRAM => 0.001,        // 1 g = 0.001 kg
                WeightUnit.POUND => 0.453592,    // 1 lb = 0.453592 kg
                _ => throw new ArgumentException("Invalid weight unit.")
            };
        }

        /// Converts value to base unit (Kilogram).
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// Converts from base unit (Kilogram) to target unit.
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            double result = baseValue / unit.GetConversionFactor();
            return Math.Round(result, 6); // preserve precision
        }
    }
}