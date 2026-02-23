using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents an immutable length value with a specific unit.
    /// All conversions normalize through base unit (INCHES).
    /// </summary>
    public class Length
    {
        private readonly double value;
        private readonly LengthUnit unit;

        /// <summary>
        /// Supported units.
        /// Conversion factors are relative to INCHES (base unit).
        /// </summary>
        public enum LengthUnit
        {
            FEET,
            INCHES,
            YARDS,
            CENTIMETERS
        }

        public Length(double value, LengthUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        // ---------------------------
        // PRIVATE HELPER METHOD
        // ---------------------------

        private double ConvertToBaseUnit()
        {
            return unit switch
            {
                LengthUnit.FEET => value * 12,
                LengthUnit.INCHES => value,
                LengthUnit.YARDS => value * 36,
                LengthUnit.CENTIMETERS => value * 0.393701,
                _ => throw new ArgumentException("Invalid unit.")
            };
        }

        // ---------------------------
        // INSTANCE CONVERSION METHOD
        // ---------------------------

        /// <summary>
        /// Converts this Length into target unit.
        /// Returns new Length instance (immutability preserved).
        /// </summary>
        public Length ConvertTo(LengthUnit targetUnit)
        {
            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.");

            double baseValue = ConvertToBaseUnit();

            double converted = targetUnit switch
            {
                LengthUnit.FEET => baseValue / 12,
                LengthUnit.INCHES => baseValue,
                LengthUnit.YARDS => baseValue / 36,
                LengthUnit.CENTIMETERS => baseValue / 0.393701,
                _ => throw new ArgumentException("Invalid unit.")
            };

            return new Length(Math.Round(converted, 6), targetUnit);
        }

        // ---------------------------
        // STATIC CONVERSION API
        // ---------------------------

        /// <summary>
        /// Static conversion method.
        /// Converts value from source unit to target unit.
        /// </summary>
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            if (!Enum.IsDefined(typeof(LengthUnit), source) ||
                !Enum.IsDefined(typeof(LengthUnit), target))
                throw new ArgumentException("Invalid unit.");

            double baseValue = source switch
            {
                LengthUnit.FEET => value * 12,
                LengthUnit.INCHES => value,
                LengthUnit.YARDS => value * 36,
                LengthUnit.CENTIMETERS => value * 0.393701,
                _ => throw new ArgumentException("Invalid unit.")
            };

            double converted = target switch
            {
                LengthUnit.FEET => baseValue / 12,
                LengthUnit.INCHES => baseValue,
                LengthUnit.YARDS => baseValue / 36,
                LengthUnit.CENTIMETERS => baseValue / 0.393701,
                _ => throw new ArgumentException("Invalid unit.")
            };

            return Math.Round(converted, 6);
        }

        // ---------------------------
        // EQUALITY (UC3 + UC4)
        // ---------------------------

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not Length other)
                return false;

            return Math.Round(ConvertToBaseUnit(), 6)
                == Math.Round(other.ConvertToBaseUnit(), 6);
        }

        public override int GetHashCode()
        {
            return Math.Round(ConvertToBaseUnit(), 6).GetHashCode();
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }
    }
}