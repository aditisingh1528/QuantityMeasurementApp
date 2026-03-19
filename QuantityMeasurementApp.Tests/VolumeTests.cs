using NUnit.Framework;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementApp.Tests
{
    public class VolumeTests
    {
        private const double EPSILON = 1e-6;

        [Test]
        public void testEquality_LitreToLitre_SameValue()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public void testEquality_LitreToMillilitre_EquivalentValue()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public void testEquality_GallonToLitre_EquivalentValue()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            var v2 = new Quantity<VolumeUnit>(3.78541, VolumeUnit.LITRE);

            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public void testConversion_LitreToMillilitre()
        {
            var v = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var converted = v.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.That(converted.Value, Is.EqualTo(1000.0).Within(EPSILON));
        }

        [Test]
        public void testConversion_GallonToLitre()
        {
            var v = new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON);
            var converted = v.ConvertTo(VolumeUnit.LITRE);

            Assert.That(converted.Value, Is.EqualTo(3.78541).Within(0.01));
        }

        [Test]
        public void testAddition_LitrePlusMillilitre()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            var result = v1.Add(v2);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
        }

        [Test]
public void testAddition_ExplicitTargetUnit()
{
    var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
    var v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
    
    var result = v1.Add(v2);

    var converted = result.ConvertTo(VolumeUnit.MILLILITRE);

    Assert.That(converted.Value, Is.EqualTo(2000.0).Within(EPSILON));
}
    }
}