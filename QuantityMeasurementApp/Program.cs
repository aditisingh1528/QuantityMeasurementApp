using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.UI;
using System;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Temperature Equality Test:");

            var t1 = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);

            Console.WriteLine($"0°C == 32°F : {t1.Equals(t2)}");

            Console.WriteLine("\nTemperature Conversion Test:");

            var boilingC = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var boilingF = boilingC.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Console.WriteLine($"100°C = {boilingF}");

            Console.WriteLine("\nUnsupported Operation Test:");

            try
            {
                t1.Add(new Quantity<TemperatureUnit>(10, TemperatureUnit.CELSIUS));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nStarting Menu...\n");

            Menu menu = new Menu();
            menu.Show();
        }
    }
}