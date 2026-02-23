using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IQuantityMeasurementService service = new QuantityMeasurementService();

            DemonstrateFeetEquality(service);
            DemonstrateInchesEquality(service);
            DemonstrateFeetInchesComparison(service);
            DemonstrateExtendedUnits(service);
        }

        private static void DemonstrateFeetEquality(IQuantityMeasurementService service)
        {
            Length l1 = new Length(1.0, Length.LengthUnit.FEET);
            Length l2 = new Length(1.0, Length.LengthUnit.FEET);

            Console.WriteLine($"Feet Equality: {service.AreLengthsEqual(l1, l2)}");
        }

        private static void DemonstrateInchesEquality(IQuantityMeasurementService service)
        {
            Length l1 = new Length(1.0, Length.LengthUnit.INCHES);
            Length l2 = new Length(1.0, Length.LengthUnit.INCHES);

            Console.WriteLine($"Inches Equality: {service.AreLengthsEqual(l1, l2)}");
        }

        private static void DemonstrateFeetInchesComparison(IQuantityMeasurementService service)
        {
            Length l1 = new Length(1.0, Length.LengthUnit.FEET);
            Length l2 = new Length(12.0, Length.LengthUnit.INCHES);

            Console.WriteLine($"Feet-Inches Equality: {service.AreLengthsEqual(l1, l2)}");
        }

        private static void DemonstrateExtendedUnits(IQuantityMeasurementService service)
        {
            Console.WriteLine("Yard to Feet: " +
                service.AreLengthsEqual(
                    new Length(1.0, Length.LengthUnit.YARDS),
                    new Length(3.0, Length.LengthUnit.FEET)));

            Console.WriteLine("Yard to Inches: " +
                service.AreLengthsEqual(
                    new Length(1.0, Length.LengthUnit.YARDS),
                    new Length(36.0, Length.LengthUnit.INCHES)));

            Console.WriteLine("Centimeter to Inches: " +
                service.AreLengthsEqual(
                    new Length(1.0, Length.LengthUnit.CENTIMETERS),
                    new Length(0.393701, Length.LengthUnit.INCHES)));
        }
    }
}