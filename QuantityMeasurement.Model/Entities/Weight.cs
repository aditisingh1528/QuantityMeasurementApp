using System;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementModel.Entities
{
    public class Weight
    {
        private const double EPSILON = 0.0001;

        public double Value { get; }
        public WeightUnit Unit { get; }

        public Weight(double value, WeightUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        private double ConvertToBase(double value, WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.GRAM => value,
                WeightUnit.KILOGRAM => value * 1000,
                WeightUnit.POUND => value * 453.592,
                _ => throw new Exception("Invalid Weight Unit")
            };
        }

        private double ConvertFromBase(double baseValue, WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.GRAM => baseValue,
                WeightUnit.KILOGRAM => baseValue / 1000,
                WeightUnit.POUND => baseValue / 453.592,
                _ => throw new Exception("Invalid Weight Unit")
            };
        }

        public Weight Add(Weight other)
        {
            double baseSum =
                ConvertToBase(this.Value, this.Unit) +
                ConvertToBase(other.Value, other.Unit);

            double result = ConvertFromBase(baseSum, this.Unit);

            return new Weight(result, this.Unit);
        }

        public static Weight Add(Weight w1, Weight w2)
        {
            return w1.Add(w2);
        }


        public Weight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = ConvertToBase(this.Value, this.Unit);
            double result = ConvertFromBase(baseValue, targetUnit);

            return new Weight(result, targetUnit);
        }

       public Weight Add(WeightUnit unit)
        {
            return new Weight(this.Value, unit);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Weight other)
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