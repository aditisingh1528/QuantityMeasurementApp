using QuantityMeasurementConsoleApp.Controllers;
using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementConsoleApp.Menu
{
    public class Menu
    {
        private readonly QuantityMeasurementController controller;

        public Menu(QuantityMeasurementController controller)
        {
            this.controller = controller;
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\n===== QUANTITY MEASUREMENT MENU =====");
                Console.WriteLine("1.  Compare Length (FEET/INCHES/YARDS/CENTIMETERS)");
                Console.WriteLine("2.  Convert Length");
                Console.WriteLine("3.  Add Length");
                Console.WriteLine("4.  Subtract Length");
                Console.WriteLine("5.  Divide Length");
                Console.WriteLine("6.  Compare Weight (KILOGRAM/GRAM/POUND)");
                Console.WriteLine("7.  Convert Weight");
                Console.WriteLine("8.  Add Weight");
                Console.WriteLine("9.  Subtract Weight");
                Console.WriteLine("10. Divide Weight");
                Console.WriteLine("11. Compare Volume (LITRE/MILLILITRE/GALLON)");
                Console.WriteLine("12. Convert Volume");
                Console.WriteLine("13. Add Volume");
                Console.WriteLine("14. Subtract Volume");
                Console.WriteLine("15. Divide Volume");
                Console.WriteLine("16. Compare Temperature (CELSIUS/FAHRENHEIT/KELVIN)");
                Console.WriteLine("17. Convert Temperature");
                Console.WriteLine("18. Add Temperature");
                Console.WriteLine("19. Subtract Temperature");
                Console.WriteLine("20. Divide Temperature");
                Console.WriteLine("21. General Compare (Any)");
                Console.WriteLine("23. View All Measurement History");
                Console.WriteLine("24. View History by Operation Type");
                Console.WriteLine("25. Show DB Pool Statistics");
                Console.WriteLine("22. Exit");

                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":  
                            Compare("Length");
                            break;
                        case "2":  
                            Convert("Length");
                            break;
                        case "3":  
                            Add("Length");
                            break;
                        case "4":  
                            Subtract("Length");
                            break;
                        case "5":  
                            Divide("Length");
                                break;
                        case "6":  
                            Compare("Weight");
                                break;
                        case "7":  
                            Convert("Weight");
                                break;
                        case "8":  
                            Add("Weight");
                                break;
                        case "9":  
                            Subtract("Weight");
                                break;
                        case "10": 
                            Divide("Weight");
                                break;
                        case "11": 
                            Compare("Volume");
                                break;
                        case "12": 
                            Convert("Volume");
                                break;
                        case "13": 
                            Add("Volume");
                                break;
                        case "14": 
                            Subtract("Volume");
                                break;
                        case "15": 
                            Divide("Volume");
                                break;
                        case "16": 
                            Compare("Temperature");
                                break;
                        case "17": 
                            Convert("Temperature");
                                break;
                        case "18": 
                            Add("Temperature");
                                break;
                        case "19": 
                            Subtract("Temperature");
                                break;
                        case "20": 
                            Divide("Temperature");
                                break;
                        case "21": 
                            GeneralCompare();
                                break;
                        case "23": 
                            ViewAllHistory();
                                break;
                        case "24": 
                            ViewHistoryByOperation();
                                break;
                        case "25": 
                            ShowPoolStats();
                                break;
                        case "22": 
                            return;
                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private QuantityDTO ReadQuantity(string type)
        {
            Console.Write($"Enter {type} value: ");
            double value = double.Parse(Console.ReadLine()!);
            Console.Write($"Enter {type} unit: ");
            string unit = Console.ReadLine()!;
            return new QuantityDTO(value, unit);
        }

        private QuantityDTO ReadAnyQuantity()
        {
            Console.Write("Enter value: ");
            double value = double.Parse(Console.ReadLine()!);
            Console.Write("Enter unit: ");
            string unit = Console.ReadLine()!;
            return new QuantityDTO(value, unit);
        }


        private void Compare(string type)
        {
            Console.WriteLine($"\n--- Compare {type} ---");
            var q1 = ReadQuantity(type);
            var q2 = ReadQuantity(type);
            bool result = controller.Compare(q1, q2);
            Console.WriteLine($"Result: {result}");
        }

        private void Convert(string type)
        {
            Console.WriteLine($"\n--- Convert {type} ---");
            var q = ReadQuantity(type);
            Console.Write("Enter target unit: ");
            string target = Console.ReadLine()!;
            var result = controller.Convert(q, target);
            Console.WriteLine($"Converted: {result.Value} {result.Unit}");
        }

        private void Add(string type)
        {
            Console.WriteLine($"\n--- Add {type} ---");
            var q1 = ReadQuantity(type);
            var q2 = ReadQuantity(type);
            var result = controller.Add(q1, q2);
            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        private void Subtract(string type)
        {
            Console.WriteLine($"\n--- Subtract {type} ---");
            var q1 = ReadQuantity(type);
            var q2 = ReadQuantity(type);
            var result = controller.Subtract(q1, q2);
            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        private void Divide(string type)
        {
            Console.WriteLine($"\n--- Divide {type} ---");
            var q1 = ReadQuantity(type);
            var q2 = ReadQuantity(type);
            double result = controller.Divide(q1, q2);
            Console.WriteLine($"Result: {result}");
        }

        private void GeneralCompare()
        {
            Console.WriteLine("\n--- General Compare ---");
            var q1 = ReadAnyQuantity();
            var q2 = ReadAnyQuantity();
            bool result = controller.Compare(q1, q2);
            Console.WriteLine($"Result: {result}");
        }

        //UC-16 Methods
        private void ViewAllHistory()
        {
            Console.WriteLine("\n--- All Measurement History ---");
            var records = controller.GetAllMeasurements();
            if (records.Count == 0)
            {
                Console.WriteLine("No measurements stored yet.");
                return;
            }
            foreach (var r in records)
                Console.WriteLine(r.ToString());
            Console.WriteLine($"Total: {records.Count} record(s).");
        }

        private void ViewHistoryByOperation()
        {
            Console.WriteLine("\n--- History by Operation Type ---");
            Console.WriteLine("Operation types: COMPARE | CONVERT | ADD | SUBTRACT | DIVIDE");
            Console.Write("Enter operation type: ");
            string op = Console.ReadLine()!.ToUpper();
            var records = controller.GetMeasurementsByOperation(op);
            if (records.Count == 0)
            {
                Console.WriteLine($"No records found for operation '{op}'.");
                return;
            }
            foreach (var r in records)
                Console.WriteLine(r.ToString());
            Console.WriteLine($"Total: {records.Count} record(s).");
        }

        private void ShowPoolStats()
        {
            Console.WriteLine("\n--- DB Connection Pool Statistics ---");
            Console.WriteLine(controller.GetPoolStatistics());
        }
    }
}
