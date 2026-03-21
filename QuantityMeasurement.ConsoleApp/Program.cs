using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementConsoleApp.Controllers;
using QuantityMeasurementConsoleApp.Menu;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementRepository.Interface;

/// <summary>
/// UC16: Program now selects the repository via RepositoryFactory
/// (reads appsettings.json – "cache" or "database").
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Quantity Measurement Application (UC16) ===");

        // UC16: choose repository from config; falls back to cache if DB unavailable
        IQuantityMeasurementRepository repository;
        try
        {
            repository = RepositoryFactory.Create();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Startup] Could not connect to database: {ex.Message}");
            Console.WriteLine("[Startup] Falling back to in-memory cache repository.");
            repository = QuantityMeasurementRepository.Cache.QuantityMeasurementCacheRepository.GetInstance();
        }

        var service    = new QuantityMeasurementService(repository);
        var controller = new QuantityMeasurementController(service);
        var menu       = new Menu(controller);
        menu.Show();

        // UC16: print pool stats and release DB resources on exit
        Console.WriteLine($"\n[Shutdown] {repository.GetPoolStatistics()}");
        repository.ReleaseResources();
    }
}
