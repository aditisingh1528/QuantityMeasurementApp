using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Mappers;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementModel.Exceptions;
using QuantityMeasurementModel.Interfaces;
using QuantityMeasurementModel.Models;
using QuantityMeasurementModel.Units;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementBusinessLayer.Services
{
    /// <summary>
    /// UC16: Repository injected via constructor; every operation auto-persists a
    ///       QuantityMeasurementEntity to the DB or cache repository.
    ///       New methods expose history queries and pool statistics.
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl()
        {
            _repository = QuantityMeasurementCacheRepository.GetInstance();
        }

        // UC16: dependency-injected constructor
        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            var c1 = GetConverter(q1.Unit);
            var c2 = GetConverter(q2.Unit);
            var m1 = QuantityMapper.ToModel(q1, c1);
            var m2 = QuantityMapper.ToModel(q2, c2);
            Validate(m1, m2);

            double b1     = c1.ConvertToBase(m1.Value);
            double b2     = c2.ConvertToBase(m2.Value);
            bool   result = Math.Abs(b1 - b2) < 0.0001;

            PersistOperation("COMPARE", m1.MeasurementType,
                q1.Value, q1.Unit, q2.Value, q2.Unit,
                result ? 1.0 : 0.0, "Boolean");

            return result;
        }

        public QuantityDTO Convert(QuantityDTO input, string targetUnit)
        {
            var from  = GetConverter(input.Unit);
            var to    = GetConverter(targetUnit);
            var model = QuantityMapper.ToModel(input, from);
            ValidateTypes(from, to);

            double baseVal = from.ConvertToBase(model.Value);
            double result  = to.ConvertFromBase(baseVal);
            var    dto     = QuantityMapper.ToDTO(new QuantityModel(result, targetUnit, to.GetMeasurementType()));

            PersistOperation("CONVERT", from.GetMeasurementType(),
                input.Value, input.Unit, null, null,
                result, targetUnit);

            return dto;
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            var c1 = GetConverter(q1.Unit);
            var c2 = GetConverter(q2.Unit);
            var m1 = QuantityMapper.ToModel(q1, c1);
            var m2 = QuantityMapper.ToModel(q2, c2);
            Validate(m1, m2);

            double baseSum = c1.ConvertToBase(m1.Value) + c2.ConvertToBase(m2.Value);
            double result  = c1.ConvertFromBase(baseSum);
            var    dto     = QuantityMapper.ToDTO(new QuantityModel(result, m1.Unit, m1.MeasurementType));

            PersistOperation("ADD", m1.MeasurementType,
                q1.Value, q1.Unit, q2.Value, q2.Unit,
                result, m1.Unit);

            return dto;
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            var c1 = GetConverter(q1.Unit);
            var c2 = GetConverter(q2.Unit);
            var m1 = QuantityMapper.ToModel(q1, c1);
            var m2 = QuantityMapper.ToModel(q2, c2);
            Validate(m1, m2);

            double baseResult = c1.ConvertToBase(m1.Value) - c2.ConvertToBase(m2.Value);
            double result     = c1.ConvertFromBase(baseResult);
            var    dto        = QuantityMapper.ToDTO(new QuantityModel(result, m1.Unit, m1.MeasurementType));

            PersistOperation("SUBTRACT", m1.MeasurementType,
                q1.Value, q1.Unit, q2.Value, q2.Unit,
                result, m1.Unit);

            return dto;
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

            double result = b1 / b2;

            PersistOperation("DIVIDE", m1.MeasurementType,
                q1.Value, q1.Unit, q2.Value, q2.Unit,
                result, "Ratio");

            return result;
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

        //UC16: repository-query pass-throughs

        public List<QuantityMeasurementEntity> GetAllMeasurements()
            => _repository.GetAllMeasurements();

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
            => _repository.GetMeasurementsByOperation(operationType);

        public string GetPoolStatistics()
            => _repository.GetPoolStatistics();


        private void PersistOperation(
            string   operationType,
            string   category,
            double   op1Value, string  op1Unit,
            double?  op2Value, string? op2Unit,
            double?  resultValue, string? resultUnit,
            string?  errorMessage = null)
        {
            try
            {
                _repository.SaveMeasurement(new QuantityMeasurementEntity(
                    operationType, category,
                    op1Value, op1Unit,
                    op2Value, op2Unit,
                    resultValue, resultUnit,
                    errorMessage));
            }
            catch (Exception ex)
            {
                // Persistence failure must NEVER crash the operation itself
                Console.WriteLine($"[Service] Warning: persistence failed – {ex.Message}");
            }
        }

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
    }
}
