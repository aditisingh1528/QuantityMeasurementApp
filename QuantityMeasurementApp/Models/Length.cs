using System;

namespace QuantityMeasurementApp.Models
{
    /// Length class represents a measurement with value and unit.
    /// UC4 extends support to YARDS and CENTIMETERS.
    /// Base unit is INCHES.
    public class Length
    {
        private readonly double value;
        private readonly LengthUnit unit;

        /// Enum representing supported units.
        public enum LengthUnit
        {
            FEET,
            INCHES,
            YARDS,
            CENTIMETERS
        }

        /// Constructor to initialize length.
        public Length(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        /// Converts measurement to base unit (INCHES).
        private double ConvertToBaseUnit()
        {
            switch (unit)
            {
                case LengthUnit.FEET:
                    return value * 12;

                case LengthUnit.INCHES:
                    return value;

                case LengthUnit.YARDS:
                    return value * 36;

                case LengthUnit.CENTIMETERS:
                    return value * 0.393701;

                default:
                    throw new ArgumentException("Invalid unit type.");
            }
        }

        /// Value-based equality comparison.
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null || GetType() != obj.GetType())
                return false;

            Length other = (Length)obj;

            return Math.Round(ConvertToBaseUnit(), 5)
                .Equals(Math.Round(other.ConvertToBaseUnit(), 5));
        }

        public override int GetHashCode()
        {
            return Math.Round(ConvertToBaseUnit(), 5).GetHashCode();
        }
    }
}