using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents an immutable length value with a specific unit.
    /// Supports conversion, equality, and addition operations.
    /// </summary>
    public class Length
    {
        private readonly double value;
        private readonly LengthUnit unit;

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

        // Base Unit Conversion

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

        // Conversion

        public Length ConvertTo(LengthUnit targetUnit)
        {
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

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            Length temp = new Length(value, source);
            return temp.ConvertTo(target).Value;
        }

        // Equality

        public override bool Equals(object? obj)
        {
            if (obj is not Length other)
                return false;

            return Math.Round(this.ConvertToBaseUnit(), 6)
                == Math.Round(other.ConvertToBaseUnit(), 6);
        }

        public override int GetHashCode()
        {
            return Math.Round(ConvertToBaseUnit(), 6).GetHashCode();
        }

        // UC6 Addition

        public Length Add(Length other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            double baseSum = this.ConvertToBaseUnit() + other.ConvertToBaseUnit();

            double convertedSum = unit switch
            {
                LengthUnit.FEET => baseSum / 12,
                LengthUnit.INCHES => baseSum,
                LengthUnit.YARDS => baseSum / 36,
                LengthUnit.CENTIMETERS => baseSum / 0.393701,
                _ => throw new ArgumentException("Invalid unit.")
            };

            return new Length(Math.Round(convertedSum, 6), this.unit);
        }

        // Addition with explicit target unit
        public Length Add(Length other, LengthUnit targetUnit)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit.");

            return AddAndConvert(other, targetUnit);
        }

        
        private Length AddAndConvert(Length other, LengthUnit targetUnit)
        {
            double baseSum = this.ConvertToBaseUnit() + other.ConvertToBaseUnit();

            double converted = ConvertFromBaseToTargetUnit(baseSum, targetUnit);

            return new Length(Math.Round(converted, 6), targetUnit);
        }

        private double ConvertFromBaseToTargetUnit(double baseValue, LengthUnit targetUnit)
        {
            return targetUnit switch
            {
                LengthUnit.FEET => baseValue / 12,
                LengthUnit.INCHES => baseValue,
                LengthUnit.YARDS => baseValue / 36,
                LengthUnit.CENTIMETERS => baseValue / 0.393701,
                _ => throw new ArgumentException("Invalid unit.")
            };
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }
    }
}