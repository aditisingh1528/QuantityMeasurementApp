using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Mappers;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementModel.Exceptions;
using QuantityMeasurementModel.Interfaces;
using QuantityMeasurementModel.Models;
using QuantityMeasurementModel.Units;
using QuantityMeasurementRepository.EFCore;
using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementBusinessLayer.Services
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementJpaRepository _jpaRepository;

        private readonly IQuantityMeasurementRepository? _legacyRepository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementJpaRepository jpaRepository)
        {
            _jpaRepository = jpaRepository;
        }

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository legacyRepository)
        {
            _legacyRepository = legacyRepository;
            _jpaRepository = new NoOpJpaRepository();
        }

        public QuantityMeasurementServiceImpl()
        {
            _jpaRepository = new NoOpJpaRepository();
        }

        public QuantityMeasurementDTO Compare(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO)
        {
            try
            {
                var c1 = GetConverter(thisQuantityDTO.Unit);
                var c2 = GetConverter(thatQuantityDTO.Unit);
                var m1 = convertDtoToModel(thisQuantityDTO, c1);
                var m2 = convertDtoToModel(thatQuantityDTO, c2);
                ValidateSameType(m1, m2);

                double b1     = c1.ConvertToBase(m1.Value);
                double b2     = c2.ConvertToBase(m2.Value);
                bool   result = Math.Abs(b1 - b2) < 0.0001;

                var entity = new QuantityMeasurementEntity(
                    thisQuantityDTO.Value, thisQuantityDTO.Unit, m1.MeasurementType,
                    thatQuantityDTO.Value, thatQuantityDTO.Unit,
                    "COMPARE",
                    result ? 1.0 : 0.0, null, result.ToString().ToLower());

                _jpaRepository.Save(entity);
                _legacyRepository?.SaveMeasurement(ToLegacyEntity(entity));

                return QuantityMeasurementDTO.FromEntity(entity);
            }
            catch (QuantityMeasurementException ex)
            {
                SaveErrorEntity(thisQuantityDTO, thatQuantityDTO, "COMPARE", ex.Message);
                throw;
            }
        }

        public QuantityMeasurementDTO Convert(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO)
        {
            try
            {
                var from  = GetConverter(thisQuantityDTO.Unit);
                var to    = GetConverter(thatQuantityDTO.Unit);
                ValidateConverterTypes(from, to);

                double baseVal = from.ConvertToBase(thisQuantityDTO.Value);
                double result  = to.ConvertFromBase(baseVal);

                var entity = new QuantityMeasurementEntity(
                    thisQuantityDTO.Value, thisQuantityDTO.Unit, from.GetMeasurementType(),
                    "CONVERT",
                    result, thatQuantityDTO.Unit);

                _jpaRepository.Save(entity);
                _legacyRepository?.SaveMeasurement(ToLegacyEntity(entity));

                return QuantityMeasurementDTO.FromEntity(entity);
            }
            catch (QuantityMeasurementException ex)
            {
                SaveErrorEntity(thisQuantityDTO, thatQuantityDTO, "CONVERT", ex.Message);
                throw;
            }
        }

        public QuantityMeasurementDTO Add(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO)
        {
            return PerformArithmetic(thisQuantityDTO, thatQuantityDTO, null, "ADD",
                (b1, b2) => b1 + b2);
        }

        public QuantityMeasurementDTO Add(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO,
                                          QuantityDTO targetUnitDTO)
        {
            return PerformArithmetic(thisQuantityDTO, thatQuantityDTO, targetUnitDTO, "ADD",
                (b1, b2) => b1 + b2);
        }

        public QuantityMeasurementDTO Subtract(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO)
        {
            return PerformArithmetic(thisQuantityDTO, thatQuantityDTO, null, "SUBTRACT",
                (b1, b2) => b1 - b2);
        }

        public QuantityMeasurementDTO Subtract(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO,
                                               QuantityDTO targetUnitDTO)
        {
            return PerformArithmetic(thisQuantityDTO, thatQuantityDTO, targetUnitDTO, "SUBTRACT",
                (b1, b2) => b1 - b2);
        }

        public QuantityMeasurementDTO Divide(QuantityDTO thisQuantityDTO, QuantityDTO thatQuantityDTO)
        {
            try
            {
                var c1 = GetConverter(thisQuantityDTO.Unit);
                var c2 = GetConverter(thatQuantityDTO.Unit);
                var m1 = convertDtoToModel(thisQuantityDTO, c1);
                var m2 = convertDtoToModel(thatQuantityDTO, c2);
                ValidateSameType(m1, m2);

                double b1 = c1.ConvertToBase(m1.Value);
                double b2 = c2.ConvertToBase(m2.Value);

                if (b2 == 0)
                    throw new QuantityMeasurementException("Divide by zero");

                double result = b1 / b2;

                var entity = new QuantityMeasurementEntity(
                    thisQuantityDTO.Value, thisQuantityDTO.Unit, m1.MeasurementType,
                    thatQuantityDTO.Value, thatQuantityDTO.Unit,
                    "DIVIDE",
                    result, "Ratio");

                _jpaRepository.Save(entity);
                _legacyRepository?.SaveMeasurement(ToLegacyEntity(entity));

                return QuantityMeasurementDTO.FromEntity(entity);
            }
            catch (QuantityMeasurementException ex)
            {
                SaveErrorEntity(thisQuantityDTO, thatQuantityDTO, "DIVIDE", ex.Message);
                throw;
            }
        }

        public List<QuantityMeasurementDTO> GetOperationHistory(string operation)
        {
            var entities = _jpaRepository.FindByOperation(operation.ToUpper());
            return QuantityMeasurementDTO.FromEntityList(entities);
        }

        public List<QuantityMeasurementDTO> GetMeasurementsByType(string type)
        {
            var entities = _jpaRepository.FindByThisMeasurementType(type);
            return QuantityMeasurementDTO.FromEntityList(entities);
        }

        public long GetOperationCount(string operation)
        {
            return _jpaRepository.CountByOperationAndIsErrorFalse(operation.ToUpper());
        }

        public List<QuantityMeasurementDTO> GetErrorHistory()
        {
            var entities = _jpaRepository.FindByIsErrorTrue();
            return QuantityMeasurementDTO.FromEntityList(entities);
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

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            if (_legacyRepository != null)
                return _legacyRepository.GetAllMeasurements();
            return _jpaRepository.FindAll();
        }

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
        {
            if (_legacyRepository != null)
                return _legacyRepository.GetMeasurementsByOperation(operationType);
            return _jpaRepository.FindByOperation(operationType);
        }

        public string GetPoolStatistics()
        {
            if (_legacyRepository != null)
                return _legacyRepository.GetPoolStatistics();
            return "EF Core repository – connection managed by DbContext.";
        }
        private QuantityMeasurementDTO PerformArithmetic(
            QuantityDTO thisQ, QuantityDTO thatQ, QuantityDTO? targetQ,
            string operationName, Func<double, double, double> arithmeticOp)
        {
            try
            {
                var c1 = GetConverter(thisQ.Unit);
                var c2 = GetConverter(thatQ.Unit);
                var m1 = convertDtoToModel(thisQ, c1);
                var m2 = convertDtoToModel(thatQ, c2);
                ValidateSameType(m1, m2);

                if (c1.GetMeasurementType() == "Temperature")
                    throw new QuantityMeasurementException(
                        "Arithmetic operations (add, subtract, divide) are not supported for Temperature. " +
                        "Use comparison or conversion instead.");

                double baseResult = arithmeticOp(
                    c1.ConvertToBase(m1.Value),
                    c2.ConvertToBase(m2.Value));

                IMeasurable resultConverter = targetQ != null
                    ? GetConverter(targetQ.Unit)
                    : c1;

                double result     = resultConverter.ConvertFromBase(baseResult);
                string resultUnit = targetQ?.Unit ?? m1.Unit;

                var entity = new QuantityMeasurementEntity(
                    thisQ.Value, thisQ.Unit, m1.MeasurementType,
                    thatQ.Value, thatQ.Unit,
                    operationName,
                    result, resultUnit);

                _jpaRepository.Save(entity);
                _legacyRepository?.SaveMeasurement(ToLegacyEntity(entity));

                return QuantityMeasurementDTO.FromEntity(entity);
            }
            catch (QuantityMeasurementException ex)
            {
                SaveErrorEntity(thisQ, thatQ, operationName, ex.Message);
                throw;
            }
        }

        private void SaveErrorEntity(QuantityDTO q1, QuantityDTO q2,
                                     string operation, string errorMessage)
        {
            try
            {
                var errorEntity = new QuantityMeasurementEntity(
                    q1.Value, q1.Unit,
                    q2.Value, q2.Unit,
                    operation, errorMessage);

                _jpaRepository.Save(errorEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Service] Warning: could not save error record – {ex.Message}");
            }
        }

        private QuantityModel convertDtoToModel(QuantityDTO dto, IMeasurable converter)
        {
            return QuantityMapper.ToModel(dto, converter);
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

            throw new QuantityMeasurementException($"Invalid unit: {unit}");
        }

        private void ValidateSameType(QuantityModel m1, QuantityModel m2)
        {
            if (m1.MeasurementType != m2.MeasurementType)
                throw new QuantityMeasurementException(
                    $"Cannot perform arithmetic between different measurement categories: " +
                    $"{m1.MeasurementType} and {m2.MeasurementType}");
        }

        private void ValidateConverterTypes(IMeasurable u1, IMeasurable u2)
        {
            if (u1.GetMeasurementType() != u2.GetMeasurementType())
                throw new QuantityMeasurementException("Different measurement types");
        }

        private static QuantityMeasurementEntity ToLegacyEntity(QuantityMeasurementEntity e)
        {
            return e;
        }

        private class NoOpJpaRepository : IQuantityMeasurementJpaRepository
        {
            public void Save(QuantityMeasurementEntity entity) { }
            public QuantityMeasurementEntity? FindById(int id) => null;
            public List<QuantityMeasurementEntity> FindAll() => new();
            public void Delete(QuantityMeasurementEntity entity) { }
            public List<QuantityMeasurementEntity> FindByOperation(string operation) => new();
            public List<QuantityMeasurementEntity> FindByThisMeasurementType(string t) => new();
            public List<QuantityMeasurementEntity> FindByCreatedAtAfter(DateTime date) => new();
            public List<QuantityMeasurementEntity> FindSuccessfulOperations(string op) => new();
            public long CountByOperationAndIsErrorFalse(string op) => 0;
            public List<QuantityMeasurementEntity> FindByIsErrorTrue() => new();
        }
    }
}
