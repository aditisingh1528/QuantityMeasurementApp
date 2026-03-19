using System;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Mappers;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Exceptions;
using QuantityMeasurementModel.Interfaces;
using QuantityMeasurementModel.Models;
using QuantityMeasurementModel.Units;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementBusinessLayer.Services
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            var c1 = GetConverter(q1.Unit);
            var c2 = GetConverter(q2.Unit);

            var m1 = QuantityMapper.ToModel(q1, c1);
            var m2 = QuantityMapper.ToModel(q2, c2);

            Validate(m1, m2);

            double b1 = c1.ConvertToBase(m1.Value);
            double b2 = c2.ConvertToBase(m2.Value);

            return Math.Abs(b1 - b2) < 0.0001;
        }

        public QuantityDTO Convert(QuantityDTO input, string targetUnit)
        {
            var from = GetConverter(input.Unit);
            var to = GetConverter(targetUnit);

            var model = QuantityMapper.ToModel(input, from);

            ValidateTypes(from, to);

            double baseVal = from.ConvertToBase(model.Value);
            double result = to.ConvertFromBase(baseVal);

            var resultModel = new QuantityModel(result, targetUnit, to.GetMeasurementType());

            return QuantityMapper.ToDTO(resultModel);
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            var c1 = GetConverter(q1.Unit);
            var c2 = GetConverter(q2.Unit);

            var m1 = QuantityMapper.ToModel(q1, c1);
            var m2 = QuantityMapper.ToModel(q2, c2);

            Validate(m1, m2);

            double baseSum =
                c1.ConvertToBase(m1.Value) +
                c2.ConvertToBase(m2.Value);

            double result = c1.ConvertFromBase(baseSum);

            var model = new QuantityModel(result, m1.Unit, m1.MeasurementType);

            return QuantityMapper.ToDTO(model);
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            var c1 = GetConverter(q1.Unit);
            var c2 = GetConverter(q2.Unit);

            var m1 = QuantityMapper.ToModel(q1, c1);
            var m2 = QuantityMapper.ToModel(q2, c2);

            Validate(m1, m2);

            double baseResult =
                c1.ConvertToBase(m1.Value) -
                c2.ConvertToBase(m2.Value);

            double result = c1.ConvertFromBase(baseResult);

            var model = new QuantityModel(result, m1.Unit, m1.MeasurementType);

            return QuantityMapper.ToDTO(model);
        }

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            var c1 = GetConverter(q1.Unit);
            var c2 = GetConverter(q2.Unit);

            var m1 = QuantityMapper.ToModel(q1, c1);
            var m2 = QuantityMapper.ToModel(q2, c2);

            Validate(m1, m2);

            double b1 = c1.ConvertToBase(m1.Value);
            double b2 = c2.ConvertToBase(m2.Value);

            if (b2 == 0)
                throw new QuantityMeasurementException("Division by zero");

            return b1 / b2;
        }

        // HELPERS

        private IMeasurable GetConverter(string unit)
        {
            if (Enum.TryParse<LengthUnit>(unit, true, out var l))
                return new LengthConverter(l);

            if (Enum.TryParse<WeightUnit>(unit, true, out var w))
                return new WeightConverter(w);

            if (Enum.TryParse<VolumeUnit>(unit, true, out var v))
                return new VolumeConverter(v);

            if (Enum.TryParse<TemperatureUnit>(unit, true, out var t))
                return new TemperatureConverter(t);

            throw new QuantityMeasurementException("Invalid unit");
        }

        private void Validate(QuantityModel m1, QuantityModel m2)
        {
            if (m1.MeasurementType != m2.MeasurementType)
                throw new QuantityMeasurementException("Different measurement types");
        }

        private void ValidateTypes(IMeasurable u1, IMeasurable u2)
        {
            if (u1.GetMeasurementType() != u2.GetMeasurementType())
                throw new QuantityMeasurementException("Different measurement types");
        }

        public bool AreLengthsEqual(Length l1, Length l2)
        {
            if (l1 == null || l2 == null)
                throw new ArgumentNullException("Length cannot be null");

            return l1.Equals(l2);
        }

        public bool AreWeightsEqual(Weight w1, Weight w2)
        {
            if (w1 == null || w2 == null)
                throw new ArgumentNullException("Weight cannot be null");

            return w1.Equals(w2);
        }
    }
}