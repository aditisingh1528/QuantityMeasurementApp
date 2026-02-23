using NUnit.Framework;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp.Tests;

[TestFixture]
public class FeetEqualityTests
{
    private IQuantityMeasurementService _measurementService = null!;

    [SetUp]
    public void Setup()
    {
        _measurementService = new QuantityMeasurementService();
    }

    [Test]
    public void AreFeetEqual_SameValue_ReturnsTrue()
    {
        Feet first = new Feet(1.0);
        Feet second = new Feet(1.0);

        bool result = _measurementService.AreFeetEqual(first, second);

        Assert.That(result, Is.True);
    }

    [Test]
    public void AreFeetEqual_DifferentValue_ReturnsFalse()
    {
        Feet first = new Feet(1.0);
        Feet second = new Feet(2.0);

        bool result = _measurementService.AreFeetEqual(first, second);

        Assert.That(result, Is.False);
    }

    [Test]
    public void AreFeetEqual_NullComparison_ThrowsException()
    {
        Feet first = new Feet(1.0);

        Assert.Throws<QuantityMeasurementException>(() =>
            _measurementService.AreFeetEqual(first, null!));
    }

    [Test]
    public void AreFeetEqual_SameReference_ReturnsTrue()
    {
        Feet first = new Feet(1.0);

        bool result = _measurementService.AreFeetEqual(first, first);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_DifferentType_ReturnsFalse()
    {
        Feet first = new Feet(1.0);

        bool result = first.Equals("InvalidType");

        Assert.That(result, Is.False);
    }

    [Test]
    public void AreInchesEqual_SameValue_ReturnsTrue()
    {
        Inches first = new Inches(1.0);
        Inches second = new Inches(1.0);

        bool result = _measurementService.AreInchesEqual(first, second);

        Assert.That(result, Is.True);
    }

    [Test]
    public void AreInchesEqual_DifferentValue_ReturnsFalse()
    {
        Inches first = new Inches(1.0);
        Inches second = new Inches(2.0);

        bool result = _measurementService.AreInchesEqual(first, second);

        Assert.That(result, Is.False);
    }

    [Test]
    public void AreInchesEqual_NullComparison_ThrowsException()
    {
        Inches first = new Inches(1.0);

        Assert.Throws<QuantityMeasurementException>(() =>
            _measurementService.AreInchesEqual(first, null!));
    }

    [Test]
    public void AreInchesEqual_SameReference_ReturnsTrue()
    {
        Inches first = new Inches(1.0);

        bool result = _measurementService.AreInchesEqual(first, first);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Inches_Equals_DifferentType_ReturnsFalse()
    {
        Inches first = new Inches(1.0);

        bool result = first.Equals("InvalidType");

        Assert.That(result, Is.False);
    }
}