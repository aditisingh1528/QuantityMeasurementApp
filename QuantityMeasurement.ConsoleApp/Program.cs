using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementConsoleApp.Controllers;
using QuantityMeasurementConsoleApp.Menu;

class Program
{
    static void Main(string[] args)
    {
        var service = new QuantityMeasurementService();
        var controller = new QuantityMeasurementController(service);

        var menu = new Menu(controller);
        menu.Show();
    }
}