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
                Console.Write("Enter first value in feet: ");
                string firstInput = Console.ReadLine();

                Console.Write("Enter second value in feet: ");
                string secondInput = Console.ReadLine();

                if (!double.TryParse(firstInput, out double firstValue) || !double.TryParse(secondInput, out double secondValue))
                {
                    throw new QuantityMeasurementException("Invalid numeric input.");
                }

                Feet firstMeasurement = new Feet(firstValue);
                Feet secondMeasurement = new Feet(secondValue);

                bool result = measurementService.AreFeetEqual(firstMeasurement, secondMeasurement);

                Console.WriteLine($"Equal: {result}");
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}