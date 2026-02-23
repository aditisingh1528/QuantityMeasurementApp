using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IQuantityMeasurementService measurementService = new QuantityMeasurementService();

            try
            {
                DemonstrateFeetEquality(measurementService);
                DemonstrateInchesEquality(measurementService);
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void DemonstrateFeetEquality(IQuantityMeasurementService service)
        {
            Feet first = new Feet(1.0);
            Feet second = new Feet(1.0);

            bool result = service.AreFeetEqual(first, second);
            Console.WriteLine($"Feet Equal: {result}");
        }

        private static void DemonstrateInchesEquality(IQuantityMeasurementService service)
        {
            Inches first = new Inches(1.0);
            Inches second = new Inches(1.0);

            bool result = service.AreInchesEqual(first, second);
            Console.WriteLine($"Inches Equal: {result}");
        }
    }
}