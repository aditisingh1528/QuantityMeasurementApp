using System;

namespace QuantityMeasurementApp.Models
{
    // UC10- Generic Quantity class replacing category-specific implementations
    public class Quantity<U> where U : IMeasurable
    {
        private readonly double value;
        private readonly U unit;

        private const double EPSILON = 1e-6;

        public Quantity(double value, U unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite");

            this.value = value;
            this.unit = unit;
        }

        public double Value => value;

        public U Unit => unit;

        // UC10- generic conversion for any measurement type
        public Quantity<U> ConvertTo(U targetUnit)
        {
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            double baseValue = unit.ConvertToBaseUnit(value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new Quantity<U>(Math.Round(converted, 6), targetUnit);
        }

        // UC10- addition returning result in first operand unit
        public Quantity<U> Add(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            double baseSum =
                unit.ConvertToBaseUnit(value) +
                other.unit.ConvertToBaseUnit(other.value);

            double result = unit.ConvertFromBaseUnit(baseSum);

            return new Quantity<U>(Math.Round(result, 6), unit);
        }

        // UC10- addition returning result in specified unit
        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            double baseSum =
                unit.ConvertToBaseUnit(value) +
                other.unit.ConvertToBaseUnit(other.value);

            double result = targetUnit.ConvertFromBaseUnit(baseSum);

            return new Quantity<U>(Math.Round(result, 6), targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not Quantity<U> other)
                return false;

            if (unit.GetType() != other.unit.GetType())
                return false;

            double thisBase = unit.ConvertToBaseUnit(value);
            double otherBase = other.unit.ConvertToBaseUnit(other.value);

            return Math.Abs(thisBase - otherBase) < EPSILON;
        }
        
        // UC10- hash code based on base unit value
        public override int GetHashCode()
        {
            double baseValue = unit.ConvertToBaseUnit(value);
            return baseValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"{value} {unit.GetUnitName()}";
        }
    }
}