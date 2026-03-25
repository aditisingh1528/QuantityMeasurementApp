using System;
using QuantityMeasurementModel.Interfaces;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementBusinessLayer.Services
{
    public class LengthConverter : IMeasurable
    {
        private readonly LengthUnit unit;

        public LengthConverter(LengthUnit unit)
        {
            this.unit = unit;
        }

        public double ConvertToBase(double value)
        {
            return unit switch
            {
                LengthUnit.FEET => value * 12,
                LengthUnit.INCHES => value,
                LengthUnit.YARDS => value * 36,
                LengthUnit.CENTIMETERS => value * 0.393701,
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }

        public double ConvertFromBase(double baseValue)
        {
            return unit switch
            {
                LengthUnit.FEET => baseValue / 12,
                LengthUnit.INCHES => baseValue,
                LengthUnit.YARDS => baseValue / 36,
                LengthUnit.CENTIMETERS => baseValue / 0.393701,
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }

        public string GetMeasurementType()
        {
            return "Length";
        }
    }
}