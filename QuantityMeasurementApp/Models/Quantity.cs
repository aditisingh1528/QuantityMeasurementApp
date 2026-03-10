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
                throw new System.ArgumentException("Unit cannot be null.");

            if (!double.IsFinite(value))
                throw new System.ArgumentException("Value must be finite.");

            this.value = value;
            this.unit = unit;
        }

        // Conversion
        public Quantity<U> ConvertTo(U targetUnit)
        {
            if (targetUnit == null)
                throw new System.ArgumentException("Target unit cannot be null.");

            double baseValue = ConvertToBase(unit, value);
            double converted = ConvertFromBase(targetUnit, baseValue);

            return new Quantity<U>(converted, targetUnit);
        }

        // Addition
        public Quantity<U> Add(Quantity<U> other)
        {
            validateArithmeticOperands(other, unit, false);

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.ADD);

            double result = ConvertFromBase(unit, baseResult);

            result = RoundTwoDecimals(result);

            return new Quantity<U>(result, unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            validateArithmeticOperands(other, targetUnit, true);

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.ADD);

            double result = ConvertFromBase(targetUnit, baseResult);

            result = RoundTwoDecimals(result);

            return new Quantity<U>(result, targetUnit);
        }

        // Subtraction
        public Quantity<U> Subtract(Quantity<U> other)
        {
            validateArithmeticOperands(other, unit, false);

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

            double result = ConvertFromBase(unit, baseResult);

            result = RoundTwoDecimals(result);

            return new Quantity<U>(result, unit);
        }

        public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
        {
            validateArithmeticOperands(other, targetUnit, true);

            double baseResult = performBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

            double result = ConvertFromBase(targetUnit, baseResult);

            result = RoundTwoDecimals(result);

            return new Quantity<U>(result, targetUnit);
        }

        // Division
        public double Divide(Quantity<U> other)
        {
            validateArithmeticOperands(other, default!, false);

            double result = performBaseArithmetic(other, ArithmeticOperation.DIVIDE);

            return result;
        }

        // Centralized Validation
        private void validateArithmeticOperands(Quantity<U> other, U targetUnit, bool targetUnitRequired)
        {
            if (other == null)
                throw new System.ArgumentException("Other quantity cannot be null.");

            if (!double.IsFinite(this.value) || !double.IsFinite(other.value))
                throw new System.ArgumentException("Values must be finite.");

            if (unit!.GetType() != other.unit!.GetType())
                throw new System.ArgumentException("Cannot operate on different unit categories.");

            if (targetUnitRequired && targetUnit == null)
                throw new System.ArgumentException("Target unit cannot be null.");
        }

        // Centralized Arithmetic
        private double performBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            double thisBase = ConvertToBase(unit, value);
            double otherBase = ConvertToBase(other.unit, other.value);

            return Compute(operation, thisBase, otherBase);
        }

        // Enum for Operations
        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        private static double Compute(ArithmeticOperation operation, double a, double b)
        {
            return operation switch
            {
                ArithmeticOperation.ADD => a + b,
                ArithmeticOperation.SUBTRACT => a - b,
                ArithmeticOperation.DIVIDE => b == 0
                    ? throw new System.ArgumentException("Division by zero.")
                    : a / b,
                _ => throw new System.ArgumentException("Invalid operation")
            };
        }

        // Rounding
        private static double RoundTwoDecimals(double value)
        {
            return Math.Round(value, 2);
        }

        // Equality
        public override bool Equals(object? obj)
        {
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

        // Unit Conversion Helpers
        private static double ConvertToBase(U unit, double value)
        {
            if (unit is LengthUnit l)
                return l.ConvertToBaseUnit(value);

            if (unit is WeightUnit w)
                return w.ConvertToBaseUnit(value);

            if (unit is VolumeUnit v)
                return v.ConvertToBaseUnit(value);

            throw new System.ArgumentException("Unsupported unit type.");
        }

        private static double ConvertFromBase(U unit, double baseValue)
        {
            if (unit is LengthUnit l)
                return l.ConvertFromBaseUnit(baseValue);

            if (unit is WeightUnit w)
                return w.ConvertFromBaseUnit(baseValue);

            if (unit is VolumeUnit v)
                return v.ConvertFromBaseUnit(baseValue);

            throw new System.ArgumentException("Unsupported unit type.");
        }
    }
}