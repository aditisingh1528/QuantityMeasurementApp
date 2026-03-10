using System;

namespace QuantityMeasurementApp.Models
{
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    public static class WeightUnitExtensions
    {
        // UC10: conversion factors relative to base unit (GRAM)
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.GRAM => 1.0,
                WeightUnit.KILOGRAM => 1000.0,   // 1 kg = 1000 g
                WeightUnit.POUND => 453.592,     // 1 lb = 453.592 g
                _ => throw new ArgumentException("Invalid weight unit.")
            };
        }

        // UC10: convert value to base unit (grams)
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        // UC10: convert from base unit (grams)
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            double result = baseValue / unit.GetConversionFactor();
            return Math.Round(result, 6);
        }

        public static string GetUnitName(this WeightUnit unit)
        {
            return unit.ToString();
        }
    }
}