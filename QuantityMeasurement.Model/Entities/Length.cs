using System;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementModel.Entities
{
    public class Length
    {
        private const double EPSILON = 0.0001;

        public double Value { get; }
        public LengthUnit Unit { get; }

        public Length(double value, LengthUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        private double ConvertToBase(double value, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => value * 12,
                LengthUnit.INCHES => value,
                LengthUnit.YARDS => value * 36,
                LengthUnit.CENTIMETERS => value * 0.393701,
                _ => throw new Exception("Invalid Length Unit")
            };
        }

        private double ConvertFromBase(double baseValue, LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => baseValue / 12,
                LengthUnit.INCHES => baseValue,
                LengthUnit.YARDS => baseValue / 36,
                LengthUnit.CENTIMETERS => baseValue / 0.393701,
                _ => throw new Exception("Invalid Length Unit")
            };
        }

        public Length Add(Length other)
        {
            double baseSum =
                ConvertToBase(this.Value, this.Unit) +
                ConvertToBase(other.Value, other.Unit);

            double result = ConvertFromBase(baseSum, this.Unit);

            return new Length(result, this.Unit);
        }

        public Length Subtract(Length other)
        {
            double baseDiff =
                ConvertToBase(this.Value, this.Unit) -
                ConvertToBase(other.Value, other.Unit);

            double result = ConvertFromBase(baseDiff, this.Unit);

            return new Length(result, this.Unit);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Length other)
                return false;

            double b1 = ConvertToBase(this.Value, this.Unit);
            double b2 = ConvertToBase(other.Value, other.Unit);

            return Math.Abs(b1 - b2) < EPSILON;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}