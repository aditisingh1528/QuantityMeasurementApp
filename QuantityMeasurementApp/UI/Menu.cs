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
                Console.WriteLine("5. Compare Two Weights");
                Console.WriteLine("6. Convert Weight");
                Console.WriteLine("7. Add Two Weights (First Unit)");
                Console.WriteLine("8. Add Two Weights (Specify Target)");
                Console.WriteLine("9. Compare Two Volumes");
                Console.WriteLine("10. Convert Volume");
                Console.WriteLine("11. Add Two Volumes");
                Console.WriteLine("12. Subtract Lengths");
                Console.WriteLine("13. Divide Lengths");
                Console.WriteLine("14. Subtract Weights");
                Console.WriteLine("15. Divide Weights");
                Console.WriteLine("16. Subtract Volumes");
                Console.WriteLine("17. Divide Volumes");
                Console.WriteLine("18. Compare Temperatures");
                Console.WriteLine("19. Convert Temperature");
                Console.WriteLine("20. Add Temperatures");
                Console.WriteLine("21. Subtract Temperatures");
                Console.WriteLine("22. Divide Temperatures");
                Console.WriteLine("23. Exit");

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
                        CompareVolumes();
                        break;

                    case "10":
                        ConvertVolume();
                        break;

                    case "11":
                        AddVolumes();
                        break;

                    case "12":
                        SubtractLengths();
                        break;

                    case "13":
                        DivideLengths();
                        break;

                    case "14":
                        SubtractWeights();
                        break;

                    case "15":
                        DivideWeights();
                        break;

                    case "16":
                        SubtractVolumes();
                        break;

                    case "17":
                        DivideVolumes();
                        break;

                    case "18":
                        CompareTemperatures();
                        break;

                    case "19":
                        ConvertTemperature();
                        break;

                    case "20":
                        AddTemperatures();
                        break;

                    case "21":
                        SubtractTemperatures();
                        break;

                    case "22":
                        DivideTemperatures();
                        break;

                    case "23":
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

        private Quantity<VolumeUnit> ReadVolume()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Select Unit:");
            Console.WriteLine("1. LITRE");
            Console.WriteLine("2. MILLILITRE");
            Console.WriteLine("3. GALLON");

            int unitChoice = int.Parse(Console.ReadLine()!);

            VolumeUnit unit = unitChoice switch
            {
                1 => VolumeUnit.LITRE,
                2 => VolumeUnit.MILLILITRE,
                3 => VolumeUnit.GALLON,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            return new Quantity<VolumeUnit>(value, unit);
        }

        private void CompareVolumes()
        {
            Console.WriteLine("\n--- Compare Volumes ---");

            var v1 = ReadVolume();
            var v2 = ReadVolume();

            bool result = v1.Equals(v2);

            Console.WriteLine($"Result: {result}");
        }

        private void ConvertVolume()
        {
            Console.WriteLine("\n--- Convert Volume ---");

            var volume = ReadVolume();

            Console.WriteLine("Select Target Unit:");
            Console.WriteLine("1. LITRE");
            Console.WriteLine("2. MILLILITRE");
            Console.WriteLine("3. GALLON");

            int unitChoice = int.Parse(Console.ReadLine()!);

            VolumeUnit target = unitChoice switch
            {
                1 => VolumeUnit.LITRE,
                2 => VolumeUnit.MILLILITRE,
                3 => VolumeUnit.GALLON,
                _ => throw new ArgumentException("Invalid unit choice.")
            };

            var converted = volume.ConvertTo(target);

            Console.WriteLine($"Converted: {converted}");
        }

        private void AddVolumes()
        {
            Console.WriteLine("\n--- Add Two Volumes ---");

            var v1 = ReadVolume();
            var v2 = ReadVolume();

            var result = v1.Add(v2);

            Console.WriteLine($"Sum: {result}");
        }

        private void SubtractLengths()
        {
            Console.WriteLine("\n--- Subtract Lengths ---");

            Length l1 = ReadLength();
            Length l2 = ReadLength();

            var q1 = new Quantity<LengthUnit>(l1.Value, l1.Unit);
            var q2 = new Quantity<LengthUnit>(l2.Value, l2.Unit);

            var result = q1.Subtract(q2);

            Console.WriteLine($"Difference: {result}");
        }

        private void DivideLengths()
        {
            Console.WriteLine("\n--- Divide Lengths ---");

            Length l1 = ReadLength();
            Length l2 = ReadLength();

            var q1 = new Quantity<LengthUnit>(l1.Value, l1.Unit);
            var q2 = new Quantity<LengthUnit>(l2.Value, l2.Unit);

            double result = q1.Divide(q2);

            Console.WriteLine($"Ratio: {result}");
        }

        private void SubtractWeights()
        {
            Console.WriteLine("\n--- Subtract Weights ---");

            Weight w1 = ReadWeight();
            Weight w2 = ReadWeight();

            var q1 = new Quantity<WeightUnit>(w1.Value, w1.Unit);
            var q2 = new Quantity<WeightUnit>(w2.Value, w2.Unit);

            var result = q1.Subtract(q2);

            Console.WriteLine($"Difference: {result}");
        }

        private void DivideWeights()
        {
            Console.WriteLine("\n--- Divide Weights ---");

            Weight w1 = ReadWeight();
            Weight w2 = ReadWeight();

            var q1 = new Quantity<WeightUnit>(w1.Value, w1.Unit);
            var q2 = new Quantity<WeightUnit>(w2.Value, w2.Unit);

            double result = q1.Divide(q2);

            Console.WriteLine($"Ratio: {result}");
        }

        private void SubtractVolumes()
        {
            Console.WriteLine("\n--- Subtract Two Volumes ---");

            var v1 = ReadVolume();
            var v2 = ReadVolume();

            var result = v1.Subtract(v2);

            Console.WriteLine($"Difference: {result}");
        }

        private void DivideVolumes()
        {
            Console.WriteLine("\n--- Divide Two Volumes ---");

            var v1 = ReadVolume();
            var v2 = ReadVolume();

            double result = v1.Divide(v2);

            Console.WriteLine($"Ratio: {result}");
        }

        //Helper Method
        private Quantity<TemperatureUnit> ReadTemperature()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);

            Console.WriteLine("Select Unit:");
            Console.WriteLine("1. CELSIUS");
            Console.WriteLine("2. FAHRENHEIT");
            Console.WriteLine("3. KELVIN");

            int unitChoice = int.Parse(Console.ReadLine()!);

            TemperatureUnit unit = unitChoice switch
            {
                1 => TemperatureUnit.CELSIUS,
                2 => TemperatureUnit.FAHRENHEIT,
                3 => TemperatureUnit.KELVIN,
                _ => throw new ArgumentException("Invalid unit")
            };

            return new Quantity<TemperatureUnit>(value, unit);
        }

        private void CompareTemperatures()
        {
            Console.WriteLine("Enter first temperature value:");
            double v1 = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter unit (CELSIUS/FAHRENHEIT/KELVIN):");
            TemperatureUnit u1 = Enum.Parse<TemperatureUnit>(Console.ReadLine(), true);

            Console.WriteLine("Enter second temperature value:");
            double v2 = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter unit (CELSIUS/FAHRENHEIT/KELVIN):");
            TemperatureUnit u2 = Enum.Parse<TemperatureUnit>(Console.ReadLine(), true);

            var t1 = new Quantity<TemperatureUnit>(v1, u1);
            var t2 = new Quantity<TemperatureUnit>(v2, u2);

            Console.WriteLine($"Are equal: {t1.Equals(t2)}");
        }

        private void ConvertTemperature()
        {
            Console.WriteLine("Enter temperature value:");
            double value = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter current unit:");
            TemperatureUnit from = Enum.Parse<TemperatureUnit>(Console.ReadLine(), true);

            Console.WriteLine("Enter target unit:");
            TemperatureUnit to = Enum.Parse<TemperatureUnit>(Console.ReadLine(), true);

            var temp = new Quantity<TemperatureUnit>(value, from);
            var result = temp.ConvertTo(to);

            Console.WriteLine($"Converted value: {result}");
        }

        private void AddTemperatures()
        {
            Console.WriteLine("\n--- Add Temperatures ---");

            try
            {
                var t1 = ReadTemperature();
                var t2 = ReadTemperature();

                var result = t1.Add(t2);

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SubtractTemperatures()
        {
            Console.WriteLine("\n--- Subtract Temperatures ---");

            try
            {
                var t1 = ReadTemperature();
                var t2 = ReadTemperature();

                var result = t1.Subtract(t2);

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DivideTemperatures()
        {
            Console.WriteLine("\n--- Divide Temperatures ---");

            try
            {
                var t1 = ReadTemperature();
                var t2 = ReadTemperature();

                var result = t1.Divide(t2);

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}