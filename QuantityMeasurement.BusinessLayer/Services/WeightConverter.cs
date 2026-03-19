using System;
using QuantityMeasurementModel.Interfaces;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementBusinessLayer.Services
{
    public class WeightConverter : IMeasurable
    {
        private readonly WeightUnit unit;

        public WeightConverter(WeightUnit unit)
        {
            this.unit = unit;
        }

        public double ConvertToBase(double value)
        {
            return unit switch
            {
                WeightUnit.GRAM => value,
                WeightUnit.KILOGRAM => value * 1000,
                WeightUnit.POUND => value * 453.592,
                _ => throw new ArgumentException("Invalid Weight Unit")
            };
        }

        public double ConvertFromBase(double baseValue)
        {
            return unit switch
            {
                WeightUnit.GRAM => baseValue,
                WeightUnit.KILOGRAM => baseValue / 1000,
                WeightUnit.POUND => baseValue / 453.592,
                _ => throw new ArgumentException("Invalid Weight Unit")
            };
        }

        public string GetMeasurementType()
        {
            return "Weight";
        }
    }
}