using System;

namespace QuantityMeasurementApp.Models
{
    /// Length class represents a measurement value with its unit.
    /// This class eliminates duplication from Feet and Inches classes (DRY principle).
    public class Length
    {
        // Numeric value of the measurement
        private readonly double value;

        // Unit of the measurement (Feet or Inches)
        private readonly LengthUnit unit;

        /// Enum representing supported length units.
        /// Conversion factor is defined relative to base unit (Feet).
        public enum LengthUnit
        {
            FEET = 1,     // Base unit
            INCHES = 12   // 12 inches = 1 foot
        }

        /// Constructor to initialize value and unit.
        public Length(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        /// Converts the length into base unit (Feet).
        private double ConvertToBaseUnit()
        {
            if (unit == LengthUnit.FEET)
                return value;

            if (unit == LengthUnit.INCHES)
                return value / 12;

            throw new ArgumentException("Invalid unit type.");
        }

        /// Overrides Equals method to compare values after conversion.
        /// Supports cross-unit comparison (1 foot == 12 inches).
        public override bool Equals(object? obj)
        {
            // Reflexive check
            if (ReferenceEquals(this, obj))
                return true;

            // Null and type check
            if (obj is null || GetType() != obj.GetType())
                return false;

            Length other = (Length)obj;

            return ConvertToBaseUnit()
                   .Equals(other.ConvertToBaseUnit());
        }

        /// Override GetHashCode as per equality contract.
        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }
    }
}