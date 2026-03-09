using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.UI
{
    /// Handles all user interaction via menu.
    public class Menu
    {
        private readonly IQuantityMeasurementService service;

        public Menu()
        {
            service = new QuantityMeasurementService();
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\n===== QUANTITY MEASUREMENT MENU =====");
                Console.WriteLine("1. Compare Two Lengths");
                Console.WriteLine("2. Convert Length");
                Console.WriteLine("3. Add Two Lengths (First Operand Unit)");
                Console.WriteLine("4. Add Two Lengths (Specify Target Unit)");
                Console.WriteLine("5. Compare Two Weights");            // NEW
                Console.WriteLine("6. Convert Weight");                  // NEW
                Console.WriteLine("7. Add Two Weights (First Unit)");
                Console.WriteLine("8. Add Two Weights (Specify Target)");
                Console.WriteLine("9. Exit");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CompareLengths();
                        break;

                    case "2":
                        ConvertLength();
                        break;

                    case "3":
                        AddLengthsUC6();
                        break;

                    case "4":
                        AddLengthsUC7();
                        break;

                    case "5":
                        CompareWeights();
                        break;

                    case "6":
                        ConvertWeight();
                        break;

                    case "7":
                        AddWeights();
                        break;

                    case "8":
                        AddWeightsWithTarget();
                        break;

                    case "9":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private Length ReadLength()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Select Unit:");
            Console.WriteLine("1. FEET");
            Console.WriteLine("2. INCHES");
            Console.WriteLine("3. YARDS");
            Console.WriteLine("4. CENTIMETERS");

            int unitChoice = int.Parse(Console.ReadLine()!);

            LengthUnit unit = unitChoice switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            return new Length(value, unit);
        }

        private void CompareLengths()
        {
            Console.WriteLine("\n--- Compare Lengths ---");
            Length l1 = ReadLength();
            Length l2 = ReadLength();

            bool result = service.AreLengthsEqual(l1, l2);
            Console.WriteLine($"Result: {result}");
        }

        private void ConvertLength()
        {
            Console.WriteLine("\n--- Convert Length ---");
            Length l = ReadLength();

            Console.WriteLine("Convert To:");
            LengthUnit target =
                (LengthUnit)int.Parse(Console.ReadLine()!);

            Length converted = l.ConvertTo(target);
            Console.WriteLine($"Converted: {converted}");
        }

        private void AddLengthsUC6()
        {
            Console.WriteLine("\n--- Add Lengths ---");
            Length l1 = ReadLength();
            Length l2 = ReadLength();

            Length sum = l1.Add(l2);
            Console.WriteLine($"Sum: {sum}");
        }

        private void AddLengthsUC7()
        {
            Console.WriteLine("\n--- Add Lengths (Specify Target Unit) ---");

            Length l1 = ReadLength();
            Length l2 = ReadLength();

            Console.WriteLine("Select Target Unit:");
            Console.WriteLine("1. FEET");
            Console.WriteLine("2. INCHES");
            Console.WriteLine("3. YARDS");
            Console.WriteLine("4. CENTIMETERS");

            int unitChoice = int.Parse(Console.ReadLine()!);

            LengthUnit target = unitChoice switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            Length sum = l1.Add(l2, target);

            Console.WriteLine($"Sum in {target}: {sum}");
        }

        private Weight ReadWeight()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Select Unit:");
            Console.WriteLine("1. KILOGRAM");
            Console.WriteLine("2. GRAM");
            Console.WriteLine("3. POUND");

            int unitChoice = int.Parse(Console.ReadLine()!);

            WeightUnit unit = unitChoice switch
            {
                1 => WeightUnit.KILOGRAM,
                2 => WeightUnit.GRAM,
                3 => WeightUnit.POUND,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            return new Weight(value, unit);
        }

        private void CompareWeights()
        {
            Console.WriteLine("\n--- Compare Weights ---");

            Weight w1 = ReadWeight();
            Weight w2 = ReadWeight();

            bool result = service.AreWeightsEqual(w1, w2);
            Console.WriteLine($"Result: {result}");
        }

        private void ConvertWeight()
        {
            Console.WriteLine("\n--- Convert Weight ---");

            Weight w = ReadWeight();

            Console.WriteLine("Select Target Unit:");
            Console.WriteLine("1. KILOGRAM");
            Console.WriteLine("2. GRAM");
            Console.WriteLine("3. POUND");

            int unitChoice = int.Parse(Console.ReadLine()!);

            WeightUnit target = unitChoice switch
            {
                1 => WeightUnit.KILOGRAM,
                2 => WeightUnit.GRAM,
                3 => WeightUnit.POUND,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            Weight converted = w.ConvertTo(target);
            Console.WriteLine($"Converted: {converted}");
        }

        private void AddWeights()
        {
            Console.WriteLine("\n--- Add Two Weights (First Unit) ---");

            Weight w1 = ReadWeight();
            Weight w2 = ReadWeight();

            Weight sum = w1.Add(w2);

            Console.WriteLine($"Sum: {sum}");
        }

        private void AddWeightsWithTarget()
        {
            Console.WriteLine("\n--- Add Two Weights (Specify Target Unit) ---");

            Weight w1 = ReadWeight();
            Weight w2 = ReadWeight();

            Console.WriteLine("Select Target Unit:");
            Console.WriteLine("1. KILOGRAM");
            Console.WriteLine("2. GRAM");
            Console.WriteLine("3. POUND");

            int unitChoice = int.Parse(Console.ReadLine()!);

            WeightUnit target = unitChoice switch
            {
                1 => WeightUnit.KILOGRAM,
                2 => WeightUnit.GRAM,
                3 => WeightUnit.POUND,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            Weight sum = w1.Add(w2, target);

            Console.WriteLine($"Sum in {target}: {sum}");
        }
    }
}