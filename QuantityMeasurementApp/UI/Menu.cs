using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.UI
{
    /// <summary>
    /// Handles all user interaction via menu.
    /// </summary>
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
                Console.WriteLine("5. Exit");
                Console.Write("Enter choice: ");

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

            Length.LengthUnit unit = unitChoice switch
            {
                1 => Length.LengthUnit.FEET,
                2 => Length.LengthUnit.INCHES,
                3 => Length.LengthUnit.YARDS,
                4 => Length.LengthUnit.CENTIMETERS,
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
            Length.LengthUnit target =
                (Length.LengthUnit)int.Parse(Console.ReadLine()!) - 1;

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

            Length.LengthUnit target = unitChoice switch
            {
                1 => Length.LengthUnit.FEET,
                2 => Length.LengthUnit.INCHES,
                3 => Length.LengthUnit.YARDS,
                4 => Length.LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            Length sum = l1.Add(l2, target);

            Console.WriteLine($"Sum in {target}: {sum}");
        }
    }
}