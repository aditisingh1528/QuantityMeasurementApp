using System;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementModel.Entities
{
    public class Quantity<TUnit>
    {
        private const double EPSILON = 0.0001;

        public double Value { get; }
        public TUnit Unit { get; }

        public Quantity(double value, TUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        private double ConvertToBase(double value, object unit)
        {
            return unit switch
            {
                LengthUnit l => l switch
                {
                    LengthUnit.FEET => value * 12,
                    LengthUnit.INCHES => value,
                    LengthUnit.YARDS => value * 36,
                    LengthUnit.CENTIMETERS => value * 0.393701,
                    _ => throw new Exception()
                },

                WeightUnit w => w switch
                {
                    WeightUnit.GRAM => value,
                    WeightUnit.KILOGRAM => value * 1000,
                    WeightUnit.POUND => value * 453.592,
                    _ => throw new Exception()
                },

                VolumeUnit v => v switch
                {
                    VolumeUnit.LITRE => value,
                    VolumeUnit.MILLILITRE => value * 0.001,
                    VolumeUnit.GALLON => value * 3.78541,
                    _ => throw new Exception()
                },

                TemperatureUnit t => t switch
                {
                    TemperatureUnit.CELSIUS => value,
                    TemperatureUnit.FAHRENHEIT => (value - 32) * 5 / 9,
                    TemperatureUnit.KELVIN => value - 273.15,
                    _ => throw new Exception()
                },

                _ => throw new Exception("Invalid Unit")
            };
        }

        private double ConvertFromBase(double baseValue, object unit)
        {
            return unit switch
            {
                LengthUnit l => l switch
                {
                    LengthUnit.FEET => baseValue / 12,
                    LengthUnit.INCHES => baseValue,
                    LengthUnit.YARDS => baseValue / 36,
                    LengthUnit.CENTIMETERS => baseValue / 0.393701,
                    _ => throw new Exception()
                },

                WeightUnit w => w switch
                {
                    WeightUnit.GRAM => baseValue,
                    WeightUnit.KILOGRAM => baseValue / 1000,
                    WeightUnit.POUND => baseValue / 453.592,
                    _ => throw new Exception()
                },

                VolumeUnit v => v switch
                {
                    VolumeUnit.LITRE => baseValue,
                    VolumeUnit.MILLILITRE => baseValue / 0.001,
                    VolumeUnit.GALLON => baseValue / 3.78541,
                    _ => throw new Exception()
                },

                TemperatureUnit t => t switch
                {
                    TemperatureUnit.CELSIUS => baseValue,
                    TemperatureUnit.FAHRENHEIT => (baseValue * 9 / 5) + 32,
                    TemperatureUnit.KELVIN => baseValue + 273.15,
                    _ => throw new Exception()
                },

                _ => throw new Exception("Invalid Unit")
            };
        }

        // Instance Add
        public Quantity<TUnit> Add(Quantity<TUnit> other)
        {
            double baseSum =
                ConvertToBase(this.Value, this.Unit) +
                ConvertToBase(other.Value, other.Unit);

            double result = ConvertFromBase(baseSum, this.Unit);

            return new Quantity<TUnit>(result, this.Unit);
        }

        // Static Add 
        public static Quantity<TUnit> Add(Quantity<TUnit> q1, Quantity<TUnit> q2)
        {
            return q1.Add(q2);
        }

        // Instance Add(unit) 
        public Quantity<TUnit> Add(TUnit unit)
        {
            return new Quantity<TUnit>(this.Value, unit);
        }

        public Quantity<TUnit> Subtract(Quantity<TUnit> other)
        {
            double baseDiff =
                ConvertToBase(this.Value, this.Unit) -
                ConvertToBase(other.Value, other.Unit);

            double result = ConvertFromBase(baseDiff, this.Unit);

            return new Quantity<TUnit>(result, this.Unit);
        }

        public double Divide(Quantity<TUnit> other)
        {
            double b1 = ConvertToBase(this.Value, this.Unit);
            double b2 = ConvertToBase(other.Value, other.Unit);

            if (b2 == 0)
                throw new Exception("Division by zero");

            return b1 / b2;
        }

        public Quantity<TUnit> ConvertTo(TUnit targetUnit)
        {
            double baseValue = ConvertToBase(this.Value, this.Unit);
            double result = ConvertFromBase(baseValue, targetUnit);

            return new Quantity<TUnit>(result, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity<TUnit> other)
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