using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementConsoleApp.Controllers;
using QuantityMeasurementConsoleApp.Menu;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Interface;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Quantity Measurement Application (UC16 Console) ===");
        Console.WriteLine("Note: for the REST API run QuantityMeasurement.WebApi instead.");

        IQuantityMeasurementRepository repository =
            QuantityMeasurementCacheRepository.GetInstance();

        var service    = new QuantityMeasurementService(repository);
        var controller = new QuantityMeasurementController(service);
        var menu       = new Menu(controller);
        menu.Show();

        Console.WriteLine($"\n[Shutdown] {repository.GetPoolStatistics()}");
        repository.ReleaseResources();
    }
}
