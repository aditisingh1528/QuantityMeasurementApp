using System;

namespace QuantityMeasurementApp.Models
{
    public class Quantity<U>
    {
        private const double Epsilon = 1e-6;

        private readonly double value;
        private readonly U unit;

        public double Value => value;
        public U Unit => unit;

        public Quantity(double value, U unit)
        {
            if (unit == null)
                throw new ArgumentException("Unit cannot be null.");

            if (!double.IsFinite(value))
                throw new ArgumentException("Value must be finite.");

            this.value = value;
            this.unit = unit;
        }

        // -------------------------
        // CONVERSION
        // -------------------------
        public Quantity<U> ConvertTo(U targetUnit)
        {
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");

            double baseValue = ConvertToBase(unit, value);
            double converted = ConvertFromBase(targetUnit, baseValue);

            return new Quantity<U>(converted, targetUnit);
        }

        // -------------------------
        // ADDITION (UC10)
        // -------------------------
        public Quantity<U> Add(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null.");

            double baseSum =
                ConvertToBase(unit, value) +
                ConvertToBase(other.unit, other.value);

            double result = ConvertFromBase(unit, baseSum);

            return new Quantity<U>(result, unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null.");

            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");

            double baseSum =
                ConvertToBase(unit, value) +
                ConvertToBase(other.unit, other.value);

            double result = ConvertFromBase(targetUnit, baseSum);

            return new Quantity<U>(result, targetUnit);
        }

        // -------------------------
        // SUBTRACTION (UC12)
        // -------------------------
        public Quantity<U> Subtract(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null.");

            double baseResult =
                ConvertToBase(unit, value) -
                ConvertToBase(other.unit, other.value);

            double result = ConvertFromBase(unit, baseResult);

            result = Math.Round(result, 2);

            return new Quantity<U>(result, unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null.");

            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");

            double baseResult =
                ConvertToBase(unit, value) -
                ConvertToBase(other.unit, other.value);

            double result = ConvertFromBase(targetUnit, baseResult);

            result = Math.Round(result, 2);

            return new Quantity<U>(result, targetUnit);
        }

        // -------------------------
        // DIVISION (UC12)
        // -------------------------
        public double Divide(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null.");

            double divisor = ConvertToBase(other.unit, other.value);

            if (Math.Abs(divisor) <= Epsilon)
                throw new System.ArgumentException("Division by zero.");

            double dividend = ConvertToBase(unit, value);

            return dividend / divisor;
        }

        // -------------------------
        // EQUALITY
        // -------------------------
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not Quantity<U> other)
                return false;

            double thisBase = ConvertToBase(unit, value);
            double otherBase = ConvertToBase(other.unit, other.value);

            return Math.Abs(thisBase - otherBase) <= Epsilon;
        }

        public override int GetHashCode()
        {
            double baseValue = ConvertToBase(unit, value);
            return baseValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"{value} {unit}";
        }

        // -------------------------
        // HELPERS
        // -------------------------
        private static double ConvertToBase(U unit, double value)
        {
            if (unit is LengthUnit l)
                return l.ConvertToBaseUnit(value);

            if (unit is WeightUnit w)
                return w.ConvertToBaseUnit(value);

            if (unit is VolumeUnit v)
                return v.ConvertToBaseUnit(value);

            throw new ArgumentException("Unsupported unit type.");
        }

        private static double ConvertFromBase(U unit, double baseValue)
        {
            if (unit is LengthUnit l)
                return l.ConvertFromBaseUnit(baseValue);

            if (unit is WeightUnit w)
                return w.ConvertFromBaseUnit(baseValue);

            if (unit is VolumeUnit v)
                return v.ConvertFromBaseUnit(baseValue);

            throw new ArgumentException("Unsupported unit type.");
        }
    }
}