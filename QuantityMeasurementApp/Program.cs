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

            Console.WriteLine("Convert 1 FEET to INCHES: " +
                demonstrateLengthConversion(1.0,
                    Length.LengthUnit.FEET,
                    Length.LengthUnit.INCHES));

            Console.WriteLine("Convert 3 YARDS to FEET: " +
                demonstrateLengthConversion(3.0,
                    Length.LengthUnit.YARDS,
                    Length.LengthUnit.FEET));

            Console.WriteLine("Convert 36 INCHES to YARDS: " +
                demonstrateLengthConversion(36.0,
                    Length.LengthUnit.INCHES,
                    Length.LengthUnit.YARDS));

            Console.WriteLine("Convert 1 CM to INCHES: " +
                demonstrateLengthConversion(1.0,
                    Length.LengthUnit.CENTIMETERS,
                    Length.LengthUnit.INCHES));
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

        public static double demonstrateLengthConversion(double value,Length.LengthUnit fromUnit,Length.LengthUnit toUnit)
        {
            return Length.Convert(value, fromUnit, toUnit);
        }

        public static Length demonstrateLengthConversion(
            Length length,
            Length.LengthUnit toUnit)
        {
            return length.ConvertTo(toUnit);
        }
    }
}