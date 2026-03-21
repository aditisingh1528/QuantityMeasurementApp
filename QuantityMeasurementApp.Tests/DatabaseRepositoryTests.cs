using NUnit.Framework;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Interface;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class DatabaseRepositoryTests
    {
        private IQuantityMeasurementRepository _repository;
        private QuantityMeasurementService _service;

        [SetUp]
        public void SetUp()
        {
            _repository = QuantityMeasurementCacheRepository.GetInstance();
            _repository.DeleteAll();
            _service = new QuantityMeasurementService(_repository);
        }

        [TearDown]
        public void TearDown()
        {
            _repository.DeleteAll();
        }


        [Test]
        public void TestSaveAndRetrieve_SingleEntity()
        {
            var entity = new QuantityMeasurementEntity(
                "COMPARE", "Length", 1.0, "FEET", 12.0, "INCHES", 1.0, "Boolean");

            _repository.SaveMeasurement(entity);

            var all = _repository.GetAllMeasurements();
            Assert.That(all.Count, Is.EqualTo(1));
            Assert.That(all[0].Operation, Is.EqualTo("COMPARE"));
        }

        [Test]
        public void TestGetAllMeasurements_ReturnsAllSaved()
        {
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD",     "Length", 1.0, "FEET"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("COMPARE", "Weight", 2.0, "KILOGRAM"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("CONVERT", "Volume", 3.0, "LITRE"));

            var all = _repository.GetAllMeasurements();
            Assert.That(all.Count, Is.EqualTo(3));
        }


        [Test]
        public void TestGetByOperation_ReturnsOnlyMatching()
        {
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD",     "Length", 1.0, "FEET"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("COMPARE", "Length", 2.0, "FEET"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD",     "Weight", 3.0, "KILOGRAM"));

            var adds = _repository.GetMeasurementsByOperation("ADD");
            Assert.That(adds.Count, Is.EqualTo(2));
            Assert.That(adds.All(e => e.Operation == "ADD"), Is.True);
        }

        [Test]
        public void TestGetByOperation_NoMatch_ReturnsEmpty()
        {
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD", "Length", 1.0, "FEET"));

            var results = _repository.GetMeasurementsByOperation("DIVIDE");
            Assert.That(results.Count, Is.EqualTo(0));
        }


        [Test]
        public void TestGetByCategory_ReturnsOnlyMatching()
        {
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD",     "Length", 1.0, "FEET"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("COMPARE", "Weight", 2.0, "KILOGRAM"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("CONVERT", "Length", 5.0, "INCHES"));

            var lengths = _repository.GetMeasurementsByCategory("Length");
            Assert.That(lengths.Count, Is.EqualTo(2));
        }


        [Test]
        public void TestGetTotalCount_ReturnsCorrectCount()
        {
            Assert.That(_repository.GetTotalCount(), Is.EqualTo(0));

            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD", "Length", 1.0, "FEET"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD", "Weight", 2.0, "KG"));

            Assert.That(_repository.GetTotalCount(), Is.EqualTo(2));
        }

        [Test]
        public void TestDeleteAll_ClearsRepository()
        {
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD", "Length", 1.0, "FEET"));
            _repository.SaveMeasurement(new QuantityMeasurementEntity("ADD", "Weight", 2.0, "KG"));

            _repository.DeleteAll();

            Assert.That(_repository.GetTotalCount(), Is.EqualTo(0));
            Assert.That(_repository.GetAllMeasurements().Count, Is.EqualTo(0));
        }


        [Test]
        public void TestService_Compare_PersistsRecord()
        {
            var q1 = new QuantityDTO(1.0, "FEET");
            var q2 = new QuantityDTO(12.0, "INCHES");

            bool result = _service.Compare(q1, q2);

            Assert.That(result, Is.True);
            Assert.That(_repository.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repository.GetAllMeasurements()[0].Operation, Is.EqualTo("COMPARE"));
        }

        [Test]
        public void TestService_Convert_PersistsRecord()
        {
            var q = new QuantityDTO(1.0, "FEET");
            _service.Convert(q, "INCHES");

            Assert.That(_repository.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repository.GetAllMeasurements()[0].Operation, Is.EqualTo("CONVERT"));
        }

        [Test]
        public void TestService_Add_PersistsRecord()
        {
            var q1 = new QuantityDTO(1.0, "FEET");
            var q2 = new QuantityDTO(12.0, "INCHES");
            _service.Add(q1, q2);

            Assert.That(_repository.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repository.GetAllMeasurements()[0].Operation, Is.EqualTo("ADD"));
        }

        [Test]
        public void TestService_Subtract_PersistsRecord()
        {
            var q1 = new QuantityDTO(2.0, "FEET");
            var q2 = new QuantityDTO(6.0, "INCHES");
            _service.Subtract(q1, q2);

            Assert.That(_repository.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repository.GetMeasurementsByOperation("SUBTRACT").Count, Is.EqualTo(1));
        }

        [Test]
        public void TestService_Divide_PersistsRecord()
        {
            var q1 = new QuantityDTO(2.0, "FEET");
            var q2 = new QuantityDTO(1.0, "FEET");
            _service.Divide(q1, q2);

            Assert.That(_repository.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repository.GetMeasurementsByOperation("DIVIDE").Count, Is.EqualTo(1));
        }

        [Test]
        public void TestService_MultipleOps_AllPersisted()
        {
            var q1 = new QuantityDTO(1.0, "FEET");
            var q2 = new QuantityDTO(12.0, "INCHES");

            _service.Compare(q1, q2);
            _service.Add(q1, q2);
            _service.Convert(q1, "INCHES");

            Assert.That(_repository.GetTotalCount(), Is.EqualTo(3));
        }


        [Test]
        public void TestGetPoolStatistics_ReturnsString()
        {
            var stats = _service.GetPoolStatistics();
            Assert.That(stats, Is.Not.Null.And.Not.Empty);
        }


        [Test]
        public void TestEntity_FullConstructor_SetsAllFields()
        {
            var entity = new QuantityMeasurementEntity(
                "ADD", "Length",
                1.0, "FEET",
                12.0, "INCHES",
                2.0, "FEET");

            Assert.That(entity.Operation,          Is.EqualTo("ADD"));
            Assert.That(entity.MeasurementCategory,Is.EqualTo("Length"));
            Assert.That(entity.Operand1Value,      Is.EqualTo(1.0));
            Assert.That(entity.Operand1Unit,       Is.EqualTo("FEET"));
            Assert.That(entity.Operand2Value,      Is.EqualTo(12.0));
            Assert.That(entity.ResultValue,        Is.EqualTo(2.0));
        }

        [Test]
        public void TestEntity_LegacyConstructor_BackwardsCompatible()
        {
            var entity = new QuantityMeasurementEntity(3.5, "METRES", "COMPARE");

            Assert.That(entity.Value,     Is.EqualTo(3.5));
            Assert.That(entity.Unit,      Is.EqualTo("METRES"));
            Assert.That(entity.Operation, Is.EqualTo("COMPARE"));
        }


        [Test]
        public void TestSqlInjection_MaliciousInput_TreatedAsLiteral()
        {
            var entity = new QuantityMeasurementEntity(
                "'; DROP TABLE QuantityMeasurements; --",
                "Length", 1.0, "FEET");

            Assert.DoesNotThrow(() => _repository.SaveMeasurement(entity));

            var saved = _repository.GetMeasurementsByOperation(
                "'; DROP TABLE QuantityMeasurements; --");
            Assert.That(saved.Count, Is.EqualTo(1));
        }
    }
}
