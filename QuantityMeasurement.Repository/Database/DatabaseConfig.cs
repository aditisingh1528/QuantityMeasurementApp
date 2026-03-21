using System.IO;
using System.Text.Json;

namespace QuantityMeasurementRepository.Database
{
    public class DatabaseConfig
    {
        private static DatabaseConfig? _instance;
        private static readonly object _lock = new();

        public string ConnectionString { get; private set; }
        public string RepositoryType { get; private set; }
        public int MaxPoolSize { get; private set; }
        public int MinPoolSize { get; private set; }
        public int ConnectionTimeout { get; private set; }

        private DatabaseConfig()
        {
            ConnectionString =
                "Server=LAPTOP-64SKNE7A\\SQLEXPRESS01;Database=QuantityMeasurementDB;Trusted_Connection=True;" +
                "TrustServerCertificate=True;";
            RepositoryType = "database";
            MaxPoolSize = 10;
            MinPoolSize = 2;
            ConnectionTimeout = 30;

            LoadFromFile();
        }

        public static DatabaseConfig GetInstance()
        {
            if (_instance == null)
                lock (_lock)
                    _instance ??= new DatabaseConfig();
            return _instance;
        }

        private void LoadFromFile()
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "appsettings.json"),
                "appsettings.json"
            };

            foreach (var path in candidates)
            {
                if (!File.Exists(path)) continue;

                try
                {
                    var json = File.ReadAllText(path);
                    using var doc = JsonDocument.Parse(json);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("ConnectionStrings", out var cs) &&
                        cs.TryGetProperty("DefaultConnection", out var conn))
                        ConnectionString = conn.GetString() ?? ConnectionString;

                    if (root.TryGetProperty("AppSettings", out var app))
                    {
                        if (app.TryGetProperty("RepositoryType", out var rt))
                            RepositoryType = rt.GetString() ?? RepositoryType;
                        if (app.TryGetProperty("MaxPoolSize", out var max))
                            MaxPoolSize = max.GetInt32();
                        if (app.TryGetProperty("MinPoolSize", out var min))
                            MinPoolSize = min.GetInt32();
                        if (app.TryGetProperty("ConnectionTimeout", out var timeout))
                            ConnectionTimeout = timeout.GetInt32();
                    }

                    Console.WriteLine($"[DatabaseConfig] Loaded configuration from: {path}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DatabaseConfig] Warning – could not parse {path}: {ex.Message}");
                }
            }

            Console.WriteLine("[DatabaseConfig] appsettings.json not found – using default configuration.");
        }

        public bool UseDatabase => RepositoryType.Equals("database", StringComparison.OrdinalIgnoreCase);

        public override string ToString() =>
            $"RepositoryType={RepositoryType} | MaxPool={MaxPoolSize} | Timeout={ConnectionTimeout}s";
    }
}
