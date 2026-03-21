using System.Data;
using System.Data.SqlClient;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementModel.Exceptions;
using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementRepository.Database
{
    /// <summary>
    /// UC16: SQL Server implementation of IQuantityMeasurementRepository using raw ADO.NET.
    /// Equivalent to QuantityMeasurementDatabaseRepository.java in the Java UC16 material.
    /// Uses parameterised queries throughout to prevent SQL injection.
    /// </summary>
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementDatabaseRepository? _instance;
        private static readonly object _lock = new();

        private readonly ConnectionPool _pool;

        // ── SQL constants ─────────────────────────────────────────────────
        private const string InsertQuery =
            @"INSERT INTO QuantityMeasurements
              (OperationType, MeasurementCategory,
               Operand1Value, Operand1Unit,
               Operand2Value, Operand2Unit,
               ResultValue,  ResultUnit,
               ErrorMessage)
              VALUES
              (@OperationType, @MeasurementCategory,
               @Operand1Value, @Operand1Unit,
               @Operand2Value, @Operand2Unit,
               @ResultValue,  @ResultUnit,
               @ErrorMessage)";

        private const string SelectAllQuery =
            "SELECT * FROM QuantityMeasurements ORDER BY Timestamp DESC";

        private const string SelectByOperationQuery =
            "SELECT * FROM QuantityMeasurements WHERE OperationType = @OperationType ORDER BY Timestamp DESC";

        private const string SelectByCategoryQuery =
            "SELECT * FROM QuantityMeasurements WHERE MeasurementCategory = @MeasurementCategory ORDER BY Timestamp DESC";

        private const string CountQuery =
            "SELECT COUNT(*) FROM QuantityMeasurements";

        private const string DeleteAllQuery =
            "DELETE FROM QuantityMeasurements";

        // ── Constructor / singleton ───────────────────────────────────────
        private QuantityMeasurementDatabaseRepository()
        {
            _pool = ConnectionPool.GetInstance();
            InitialiseSchema();
            Console.WriteLine("[DatabaseRepository] Initialised and schema verified.");
        }

        public static QuantityMeasurementDatabaseRepository GetInstance()
        {
            if (_instance == null)
                lock (_lock)
                    _instance ??= new QuantityMeasurementDatabaseRepository();
            return _instance;
        }

        // ── Schema initialisation ─────────────────────────────────────────
        /// <summary>
        /// Creates the QuantityMeasurements and QuantityMeasurementHistory tables
        /// if they do not already exist, then ensures the indexes are present.
        /// This mirrors schema.sql from the Java UC16 material.
        /// </summary>
        private void InitialiseSchema()
        {
            const string createMeasurements = @"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.tables WHERE name = 'QuantityMeasurements')
                BEGIN
                    CREATE TABLE QuantityMeasurements (
                        Id                  INT IDENTITY(1,1) PRIMARY KEY,
                        OperationType       NVARCHAR(50)  NOT NULL,
                        MeasurementCategory NVARCHAR(50)  NOT NULL,
                        Operand1Value       FLOAT         NOT NULL,
                        Operand1Unit        NVARCHAR(50)  NOT NULL,
                        Operand2Value       FLOAT         NULL,
                        Operand2Unit        NVARCHAR(50)  NULL,
                        ResultValue         FLOAT         NULL,
                        ResultUnit          NVARCHAR(50)  NULL,
                        ErrorMessage        NVARCHAR(500) NULL,
                        Timestamp           DATETIME      DEFAULT GETDATE()
                    );
                    CREATE INDEX IDX_OperationType       ON QuantityMeasurements(OperationType);
                    CREATE INDEX IDX_MeasurementCategory ON QuantityMeasurements(MeasurementCategory);
                    CREATE INDEX IDX_Timestamp           ON QuantityMeasurements(Timestamp);
                END";

            const string createHistory = @"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.tables WHERE name = 'QuantityMeasurementHistory')
                BEGIN
                    CREATE TABLE QuantityMeasurementHistory (
                        HistoryId           INT IDENTITY(1,1) PRIMARY KEY,
                        MeasurementId       INT,
                        OperationType       NVARCHAR(50),
                        MeasurementCategory NVARCHAR(50),
                        Operand1Value       FLOAT,
                        Operand1Unit        NVARCHAR(50),
                        Operand2Value       FLOAT NULL,
                        Operand2Unit        NVARCHAR(50) NULL,
                        ResultValue         FLOAT NULL,
                        ResultUnit          NVARCHAR(50) NULL,
                        ErrorMessage        NVARCHAR(500) NULL,
                        Timestamp           DATETIME,
                        AuditAction         NVARCHAR(50),
                        AuditTimestamp      DATETIME DEFAULT GETDATE()
                    );
                END";

            const string createTrigger = @"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.triggers WHERE name = 'TRG_InsertMeasurementHistory')
                BEGIN
                    EXEC('
                    CREATE TRIGGER TRG_InsertMeasurementHistory
                    ON QuantityMeasurements
                    AFTER INSERT
                    AS
                    BEGIN
                        INSERT INTO QuantityMeasurementHistory
                            (MeasurementId, OperationType, MeasurementCategory,
                             Operand1Value, Operand1Unit, Operand2Value, Operand2Unit,
                             ResultValue, ResultUnit, ErrorMessage, Timestamp, AuditAction)
                        SELECT
                            Id, OperationType, MeasurementCategory,
                            Operand1Value, Operand1Unit, Operand2Value, Operand2Unit,
                            ResultValue, ResultUnit, ErrorMessage, Timestamp, ''INSERT''
                        FROM inserted;
                    END');
                END";

            SqlConnection? conn = null;
            try
            {
                conn = _pool.GetConnection();
                ExecuteNonQuery(conn, createMeasurements);
                ExecuteNonQuery(conn, createHistory);
                ExecuteNonQuery(conn, createTrigger);
            }
            catch (Exception ex)
            {
                throw DatabaseException.QueryFailed("InitialiseSchema", ex);
            }
            finally
            {
                _pool.ReleaseConnection(conn);
            }
        }

        // ── IQuantityMeasurementRepository — UC15 methods ─────────────────
        public void SaveMeasurement(QuantityMeasurementEntity entity)
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.GetConnection();
                using var cmd = new SqlCommand(InsertQuery, conn);

                cmd.Parameters.AddWithValue("@OperationType",       entity.Operation ?? "UNKNOWN");
                cmd.Parameters.AddWithValue("@MeasurementCategory", entity.MeasurementCategory ?? string.Empty);
                cmd.Parameters.AddWithValue("@Operand1Value",       entity.Operand1Value);
                cmd.Parameters.AddWithValue("@Operand1Unit",        entity.Operand1Unit ?? entity.Unit ?? string.Empty);
                cmd.Parameters.AddWithValue("@Operand2Value",       (object?)entity.Operand2Value ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Operand2Unit",        (object?)entity.Operand2Unit  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ResultValue",         (object?)entity.ResultValue   ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ResultUnit",          (object?)entity.ResultUnit    ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ErrorMessage",        (object?)entity.ErrorMessage  ?? DBNull.Value);

                cmd.ExecuteNonQuery();
                Console.WriteLine($"[DatabaseRepository] Saved: {entity.Operation} | {entity.Operand1Value} {entity.Operand1Unit}");
            }
            catch (DatabaseException) { throw; }
            catch (Exception ex)
            {
                throw DatabaseException.QueryFailed("SaveMeasurement", ex);
            }
            finally
            {
                _pool.ReleaseConnection(conn);
            }
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.GetConnection();
                using var cmd = new SqlCommand(SelectAllQuery, conn);
                using var reader = cmd.ExecuteReader();
                var results = MapResults(reader);
                Console.WriteLine($"[DatabaseRepository] Retrieved {results.Count} measurement(s).");
                return results;
            }
            catch (DatabaseException) { throw; }
            catch (Exception ex)
            {
                throw DatabaseException.QueryFailed("GetAllMeasurements", ex);
            }
            finally
            {
                _pool.ReleaseConnection(conn);
            }
        }

        // ── IQuantityMeasurementRepository — UC16 methods ─────────────────
        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.GetConnection();
                using var cmd = new SqlCommand(SelectByOperationQuery, conn);
                cmd.Parameters.AddWithValue("@OperationType", operationType);
                using var reader = cmd.ExecuteReader();
                return MapResults(reader);
            }
            catch (DatabaseException) { throw; }
            catch (Exception ex)
            {
                throw DatabaseException.QueryFailed($"GetMeasurementsByOperation({operationType})", ex);
            }
            finally
            {
                _pool.ReleaseConnection(conn);
            }
        }

        public List<QuantityMeasurementEntity> GetMeasurementsByCategory(string category)
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.GetConnection();
                using var cmd = new SqlCommand(SelectByCategoryQuery, conn);
                cmd.Parameters.AddWithValue("@MeasurementCategory", category);
                using var reader = cmd.ExecuteReader();
                return MapResults(reader);
            }
            catch (DatabaseException) { throw; }
            catch (Exception ex)
            {
                throw DatabaseException.QueryFailed($"GetMeasurementsByCategory({category})", ex);
            }
            finally
            {
                _pool.ReleaseConnection(conn);
            }
        }

        public int GetTotalCount()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.GetConnection();
                using var cmd = new SqlCommand(CountQuery, conn);
                return (int)(cmd.ExecuteScalar() ?? 0);
            }
            catch (DatabaseException) { throw; }
            catch (Exception ex)
            {
                throw DatabaseException.QueryFailed("GetTotalCount", ex);
            }
            finally
            {
                _pool.ReleaseConnection(conn);
            }
        }

        public void DeleteAll()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.GetConnection();
                using var cmd = new SqlCommand(DeleteAllQuery, conn);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"[DatabaseRepository] Deleted {rows} measurement(s).");
            }
            catch (DatabaseException) { throw; }
            catch (Exception ex)
            {
                throw DatabaseException.QueryFailed("DeleteAll", ex);
            }
            finally
            {
                _pool.ReleaseConnection(conn);
            }
        }

        public string GetPoolStatistics() => _pool.GetStatistics();

        public void ReleaseResources()
        {
            _pool.CloseAll();
            Console.WriteLine("[DatabaseRepository] All pool connections released.");
        }

        // ── Private helpers ───────────────────────────────────────────────
        private static List<QuantityMeasurementEntity> MapResults(SqlDataReader reader)
        {
            var list = new List<QuantityMeasurementEntity>();
            while (reader.Read())
            {
                var e = new QuantityMeasurementEntity(
                    operationType:       reader["OperationType"].ToString()       ?? string.Empty,
                    measurementCategory: reader["MeasurementCategory"].ToString() ?? string.Empty,
                    operand1Value:       Convert.ToDouble(reader["Operand1Value"]),
                    operand1Unit:        reader["Operand1Unit"].ToString()        ?? string.Empty,
                    operand2Value:       reader["Operand2Value"] == DBNull.Value ? null : Convert.ToDouble(reader["Operand2Value"]),
                    operand2Unit:        reader["Operand2Unit"]  == DBNull.Value ? null : reader["Operand2Unit"].ToString(),
                    resultValue:         reader["ResultValue"]   == DBNull.Value ? null : Convert.ToDouble(reader["ResultValue"]),
                    resultUnit:          reader["ResultUnit"]    == DBNull.Value ? null : reader["ResultUnit"].ToString(),
                    errorMessage:        reader["ErrorMessage"]  == DBNull.Value ? null : reader["ErrorMessage"].ToString()
                );
                e.Id        = Convert.ToInt32(reader["Id"]);
                e.Timestamp = Convert.ToDateTime(reader["Timestamp"]);
                list.Add(e);
            }
            return list;
        }

        private static void ExecuteNonQuery(SqlConnection conn, string sql)
        {
            using var cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
