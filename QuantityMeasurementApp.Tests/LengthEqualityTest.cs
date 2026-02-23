using System;
using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    public class LengthEqualityTests
    {
        private IQuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        [Test]
        public void testEquality_FeetToFeet_SameValue()
        {
            var l1 = new Length(1.0, Length.LengthUnit.FEET);
            var l2 = new Length(1.0, Length.LengthUnit.FEET);

            Assert.That(service.AreLengthsEqual(l1, l2), Is.True);
        }

        [Test]
        public void testEquality_InchToInch_SameValue()
        {
            var l1 = new Length(1.0, Length.LengthUnit.INCHES);
            var l2 = new Length(1.0, Length.LengthUnit.INCHES);
 
            Assert.That(service.AreLengthsEqual(l1, l2), Is.True);
        }

        [Test]
        public void testEquality_FeetToInch_EquivalentValue()
        {
            var l1 = new Length(1.0, Length.LengthUnit.FEET);
            var l2 = new Length(12.0, Length.LengthUnit.INCHES);

            Assert.That(service.AreLengthsEqual(l1, l2), Is.True);
        }

        [Test]
        public void testEquality_DifferentValues()
        {
            var l1 = new Length(1.0, Length.LengthUnit.FEET);
            var l2 = new Length(2.0, Length.LengthUnit.FEET);

            Assert.That(service.AreLengthsEqual(l1, l2), Is.False);
        }

        [Test]
        public void testEquality_SameReference()
        {
            var l1 = new Length(1.0, Length.LengthUnit.FEET);

            Assert.That(service.AreLengthsEqual(l1, l1), Is.True);
        }

        [Test]
        public void testEquality_NullComparison()
        {
            var l1 = new Length(1.0, Length.LengthUnit.FEET);

            Assert.Throws<ArgumentNullException>(() =>
                service.AreLengthsEqual(l1, null!));
        }

        [Test]
        public void testEquality_YardToFeet_EquivalentValue()
        {
            var l1 = new Length(1.0, Length.LengthUnit.YARDS);
            var l2 = new Length(3.0, Length.LengthUnit.FEET);

            Assert.That(l1.Equals(l2), Is.True);
        }

        [Test]
        public void testEquality_YardToInches_EquivalentValue()
        {
            var l1 = new Length(1.0, Length.LengthUnit.YARDS);
            var l2 = new Length(36.0, Length.LengthUnit.INCHES);

            Assert.That(l1.Equals(l2), Is.True);
        }

        [Test]
        public void testEquality_centimetersToInches_EquivalentValue()
        {
            var l1 = new Length(1.0, Length.LengthUnit.CENTIMETERS);
            var l2 = new Length(0.393701, Length.LengthUnit.INCHES);

            Assert.That(l1.Equals(l2), Is.True);
        }

        [Test]
        public void testEquality_YardToFeet_NonEquivalentValue()
        {
            var l1 = new Length(1.0, Length.LengthUnit.YARDS);
            var l2 = new Length(2.0, Length.LengthUnit.FEET);

            Assert.That(l1.Equals(l2), Is.False);
        }

        [Test]
        public void testEquality_MultiUnit_TransitiveProperty()
        {
            var yard = new Length(1.0, Length.LengthUnit.YARDS);
            var feet = new Length(3.0, Length.LengthUnit.FEET);
            var inches = new Length(36.0, Length.LengthUnit.INCHES);

            Assert.That(yard.Equals(feet), Is.True);
            Assert.That(feet.Equals(inches), Is.True);
            Assert.That(yard.Equals(inches), Is.True);
        }
    }
}