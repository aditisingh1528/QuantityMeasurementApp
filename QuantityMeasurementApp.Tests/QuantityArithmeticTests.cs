using NUnit.Framework;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementModel.Units;

namespace QuantityMeasurementApp.Tests
{
    public class QuantityArithmeticTests
    {
        private const double EPSILON = 0.01;

        [Test]
        public void testSubtraction_LitreMinusMillilitre()
        {
            var v1 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(500.0, VolumeUnit.MILLILITRE);

            var result = v1.Subtract(v2);

            Assert.That(result.Value, Is.EqualTo(4.5).Within(EPSILON));
        }

        [Test]
        public void testSubtraction_ResultingInZero()
        {
            var v1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            var result = v1.Subtract(v2);

            Assert.That(result.Value, Is.EqualTo(0.0).Within(EPSILON));
        }

        [Test]
        public void testDivision_LitreByLitre()
        {
            var v1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE);

            double result = v1.Divide(v2);

            Assert.That(result, Is.EqualTo(2.0));
        }

        [Test]
        public void testDivision_CrossUnit()
        {
            var v1 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);
            var v2 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);

            double result = v1.Divide(v2);

            Assert.That(result, Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testDivision_ByZero()
        {
            var v1 = new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(0.0, VolumeUnit.LITRE);

            Assert.Throws<System.ArgumentException>(() => v1.Divide(v2));
        }
    }
}   