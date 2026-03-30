using System;
using QuantityMeasurementModel.Interfaces;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementBusinessLayer.Services
{
    public class VolumeConverter : IMeasurable
    {
        private readonly VolumeUnit unit;

        public VolumeConverter(VolumeUnit unit)
        {
            this.unit = unit;
        }

        public double ConvertToBase(double value)
        {
            return unit switch
            {
                VolumeUnit.LITRE => value,
                VolumeUnit.MILLILITRE => value * 0.001,
                VolumeUnit.GALLON => value * 3.78541,
                _ => throw new ArgumentException("Invalid Volume Unit")
            };
        }

        public double ConvertFromBase(double baseValue)
        {
            return unit switch
            {
                VolumeUnit.LITRE => baseValue,
                VolumeUnit.MILLILITRE => baseValue / 0.001,
                VolumeUnit.GALLON => baseValue / 3.78541,
                _ => throw new ArgumentException("Invalid Volume Unit")
            };
        }

        public string GetMeasurementType()
        {
            return "Volume";
        }
    }
}