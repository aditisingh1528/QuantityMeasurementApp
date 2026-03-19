using System;
using QuantityMeasurementModel.Interfaces;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementBusinessLayer.Services
{
    public class TemperatureConverter : IMeasurable
    {
        private readonly TemperatureUnit unit;

        public TemperatureConverter(TemperatureUnit unit)
        {
            this.unit = unit;
        }

        public double ConvertToBase(double value)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => value,
                TemperatureUnit.FAHRENHEIT => (value - 32) * 5 / 9,
                TemperatureUnit.KELVIN => value - 273.15,
                _ => throw new ArgumentException("Invalid Temperature Unit")
            };
        }

        public double ConvertFromBase(double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => baseValue,
                TemperatureUnit.FAHRENHEIT => (baseValue * 9 / 5) + 32,
                TemperatureUnit.KELVIN => baseValue + 273.15,
                _ => throw new ArgumentException("Invalid Temperature Unit")
            };
        }

        public string GetMeasurementType()
        {
            return "Temperature";
        }
    }
}