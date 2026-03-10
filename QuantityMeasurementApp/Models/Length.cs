using System;

namespace QuantityMeasurementApp.Models
{
    public class Length
    {
        private readonly double value;
        private readonly LengthUnit unit;

        public Length(double value, LengthUnit unit)
        {
            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            if (!Enum.IsDefined(typeof(LengthUnit), unit))
                throw new ArgumentException("Invalid unit.");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;
        public LengthUnit Unit => unit;

        // Convert to target unit (Delegates to LengthUnit)
        public Length ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = unit.ConvertToBaseUnit(value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new Length(converted, targetUnit);
        }

        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        // Equality
        public override bool Equals(object? obj)
        {
            if (obj is not Length other)
                return false;

            double thisBase = unit.ConvertToBaseUnit(value);
            double otherBase = other.unit.ConvertToBaseUnit(other.value);

            return Math.Round(thisBase, 6) == Math.Round(otherBase, 6);
        }

        public override int GetHashCode()
        {
            return unit.ConvertToBaseUnit(value).GetHashCode();
        }

        // Addition (UC6)
        public Length Add(Length other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            double baseSum =
                unit.ConvertToBaseUnit(value) +
                other.unit.ConvertToBaseUnit(other.value);

            double result = unit.ConvertFromBaseUnit(baseSum);

            return new Length(result, unit);
        }

        // Addition with target unit (UC7)
        public Length Add(Length other, LengthUnit targetUnit)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            double baseSum =
                unit.ConvertToBaseUnit(value) +
                other.unit.ConvertToBaseUnit(other.value);

            double result = targetUnit.ConvertFromBaseUnit(baseSum);

            return new Length(result, targetUnit);
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }
    }
}