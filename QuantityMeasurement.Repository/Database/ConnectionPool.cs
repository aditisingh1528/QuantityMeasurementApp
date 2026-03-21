using System.Data.SqlClient;
using QuantityMeasurementModel.Exceptions;

namespace QuantityMeasurementRepository.Database
{
    public class ConnectionPool : IDisposable
    {
        private static ConnectionPool? _instance;
        private static readonly object _instanceLock = new();

        private readonly List<SqlConnection> _availableConnections = new();
        private readonly List<SqlConnection> _usedConnections = new();
        private readonly object _poolLock = new();

        private readonly string _connectionString;
        private readonly int _poolSize;
        private bool _disposed;

        private ConnectionPool()
        {
            var config = DatabaseConfig.GetInstance();
            _connectionString = config.ConnectionString;
            _poolSize = config.MaxPoolSize;

            for (int i = 0; i < config.MinPoolSize; i++)
            {
                try
                {
                    _availableConnections.Add(CreateConnection());
                }
                catch
                {
                }
            }

            Console.WriteLine($"[ConnectionPool] Initialised with {_availableConnections.Count} connection(s). Pool size: {_poolSize}");
        }

        public static ConnectionPool GetInstance()
        {
            if (_instance == null)
                lock (_instanceLock)
                    _instance ??= new ConnectionPool();
            return _instance;
        }

        //Acquire a connection from the pool (creates one if needed)
        public SqlConnection GetConnection()
        {
            lock (_poolLock)
            {
                if (_availableConnections.Count > 0)
                {
                    var conn = _availableConnections[^1];
                    _availableConnections.RemoveAt(_availableConnections.Count - 1);

                    // Reconnect if the connection was closed
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        try { conn.Open(); }
                        catch { conn = CreateConnection(); }
                    }

                    _usedConnections.Add(conn);
                    return conn;
                }

                if (_usedConnections.Count < _poolSize)
                {
                    var conn = CreateConnection();
                    _usedConnections.Add(conn);
                    return conn;
                }

                throw new DatabaseException(
                    $"Connection pool exhausted. Max size: {_poolSize}. " +
                    "All connections are in use.");
            }
        }

        /// Return a connection back to the pool.
        public void ReleaseConnection(SqlConnection? connection)
        {
            if (connection == null) return;

            lock (_poolLock)
            {
                _usedConnections.Remove(connection);

                if (_availableConnections.Count < _poolSize)
                    _availableConnections.Add(connection);
                else
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public int AvailableCount { get { lock (_poolLock) return _availableConnections.Count; } }
        public int UsedCount { get { lock (_poolLock) return _usedConnections.Count; } }
        public int TotalCount { get { lock (_poolLock) return _availableConnections.Count + _usedConnections.Count; } }

        public string GetStatistics() =>
            $"Pool[Available={AvailableCount}, InUse={UsedCount}, Total={TotalCount}, Max={_poolSize}]";

        private SqlConnection CreateConnection()
        {
            try
            {
                var conn = new SqlConnection(_connectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                throw DatabaseException.ConnectionFailed(_connectionString, ex);
            }
        }

        public void CloseAll()
        {
            lock (_poolLock)
            {
                foreach (var c in _availableConnections) { try { c.Close(); c.Dispose(); } catch { } }
                foreach (var c in _usedConnections) { try { c.Close(); c.Dispose(); } catch { } }
                _availableConnections.Clear();
                _usedConnections.Clear();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            CloseAll();
            _disposed = true;
        }
    }
}
