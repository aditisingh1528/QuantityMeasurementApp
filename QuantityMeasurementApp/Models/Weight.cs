using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents a weight measurement.
    /// Immutable value object.
    /// </summary>
    public sealed class Weight
    {
        private const double Epsilon = 1e-6;

        public double Value { get; }
        public WeightUnit Unit { get; }

        public Weight(double value, WeightUnit unit)
        {

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value.");

            Value = value;
            Unit = unit;
        }

        // Equality
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null || obj.GetType() != typeof(Weight))
                return false;

            var other = (Weight)obj;

            double thisBase = Unit.ConvertToBaseUnit(Value);
            double otherBase = other.Unit.ConvertToBaseUnit(other.Value);

            return Math.Abs(thisBase - otherBase) < Epsilon;
        }

        public override int GetHashCode()
        {
            double baseValue = Unit.ConvertToBaseUnit(Value);
            return baseValue.GetHashCode();
        }

        // Conversion
        
        public Weight ConvertTo(WeightUnit targetUnit)
        {

            double baseValue = Unit.ConvertToBaseUnit(Value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new Weight(converted, targetUnit);
        }

        // Addition (implicit target)
        
        public Weight Add(Weight other)
        {
            if (other == null)
                throw new ArgumentException("Other weight cannot be null.");

            double baseSum =
                Unit.ConvertToBaseUnit(Value) +
                other.Unit.ConvertToBaseUnit(other.Value);

            double result = Unit.ConvertFromBaseUnit(baseSum);

            return new Weight(result, Unit);
        }

        // Addition (explicit target)
        
        public Weight Add(Weight other, WeightUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Other weight cannot be null.");

            double baseSum =
                Unit.ConvertToBaseUnit(Value) +
                other.Unit.ConvertToBaseUnit(other.Value);

            double result = targetUnit.ConvertFromBaseUnit(baseSum);

            return new Weight(result, targetUnit);
        }

        public override string ToString()
        {
            return $"{Math.Round(Value, 6)} {Unit}";
        }
    }
}