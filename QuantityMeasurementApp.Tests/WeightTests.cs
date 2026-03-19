using System;
using NUnit.Framework;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementModel.Units;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Services;

namespace QuantityMeasurementApp.Tests
{
    public class WeightTests
    {
        private IQuantityMeasurementService service;
        private const double Epsilon = 1e-5;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // Equality Tests

        [Test]
        public void testEquality_KilogramToKilogram_SameValue()
        {
            var w1 = new Weight(1.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(1.0, WeightUnit.KILOGRAM);

            Assert.That(service.AreWeightsEqual(w1, w2), Is.True);
        }

        [Test]
        public void testEquality_KilogramToGram_EquivalentValue()
        {
            var w1 = new Weight(1.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(1000.0, WeightUnit.GRAM);

            Assert.That(service.AreWeightsEqual(w1, w2), Is.True);
        }

        [Test]
        public void testEquality_KilogramToPound_EquivalentValue()
        {
            var w1 = new Weight(1.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(2.20462, WeightUnit.POUND);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void testEquality_DifferentValues()
        {
            var w1 = new Weight(1.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(2.0, WeightUnit.KILOGRAM);

            Assert.That(service.AreWeightsEqual(w1, w2), Is.False);
        }

        [Test]
        public void testEquality_SameReference()
        {
            var w = new Weight(1.0, WeightUnit.KILOGRAM);

            Assert.That(service.AreWeightsEqual(w, w), Is.True);
        }

        [Test]
        public void testEquality_NullComparison()
        {
            var w = new Weight(1.0, WeightUnit.KILOGRAM);

            Assert.Throws<ArgumentNullException>(() =>
                service.AreWeightsEqual(w, null!));
        }

        [Test]
        public void testEquality_WeightVsLength_Incompatible()
        {
            var weight = new Weight(1.0, WeightUnit.KILOGRAM);
            var length = new Length(1.0, LengthUnit.FEET);

            Assert.That(weight.Equals(length), Is.False);
        }

        // Conversion Tests
        [Test]
        public void testConversion_KilogramToGram()
        {
            var w = new Weight(1.0, WeightUnit.KILOGRAM);
            var converted = w.ConvertTo(WeightUnit.GRAM);

            Assert.That(converted.Value, Is.EqualTo(1000.0).Within(Epsilon));
        }

        [Test]
        public void testConversion_PoundToKilogram()
        {
            var w = new Weight(2.20462, WeightUnit.POUND);
            var converted = w.ConvertTo(WeightUnit.KILOGRAM);

            Assert.That(converted.Value, Is.EqualTo(1.0).Within(0.01));
        }

        [Test]
        public void testConversion_RoundTrip()
        {
            var original = new Weight(1.5, WeightUnit.KILOGRAM);
            var roundTrip = original
                .ConvertTo(WeightUnit.GRAM)
                .ConvertTo(WeightUnit.KILOGRAM);

            Assert.That(roundTrip.Value, Is.EqualTo(1.5).Within(Epsilon));
        }
        
        // Addition Tests
        [Test]
        public void testAddition_SameUnit()
        {
            var w1 = new Weight(1.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(2.0, WeightUnit.KILOGRAM);

            var result = w1.Add(w2);

            Assert.That(result.Value, Is.EqualTo(3.0).Within(Epsilon));
        }

        [Test]
        public void testAddition_CrossUnit_KgPlusGram()
        {
            var w1 = new Weight(1.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(1000.0, WeightUnit.GRAM);

            var result = w1.Add(w2);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(Epsilon));
        }

        [Test]
        public void testAddition_ExplicitTargetUnit()
        {
            var w1 = new Weight(1.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(1000.0, WeightUnit.GRAM);

            var result = w1.Add(w2);

            var converted = result.ConvertTo(WeightUnit.GRAM);

            Assert.That(converted.Value, Is.EqualTo(2000.0).Within(Epsilon));
        }
        [Test]
        public void testAddition_WithNegativeValue()
        {
            var w1 = new Weight(5.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(-2000.0, WeightUnit.GRAM);

            var result = w1.Add(w2);

            Assert.That(result.Value, Is.EqualTo(3.0).Within(Epsilon));
        }

        [Test]
        public void testAddition_WithZero()
        {
            var w1 = new Weight(5.0, WeightUnit.KILOGRAM);
            var w2 = new Weight(0.0, WeightUnit.GRAM);

            var result = w1.Add(w2);

            Assert.That(result.Value, Is.EqualTo(5.0).Within(Epsilon));
        }
    }
}