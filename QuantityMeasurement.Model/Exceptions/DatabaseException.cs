namespace QuantityMeasurementModel.Exceptions
{
    // Thrown when a database operation fails.
    public class DatabaseException : QuantityMeasurementException
    {
        public DatabaseException(string message) : base(message) { }

        public DatabaseException(string message, Exception inner)
            : base(message, inner) { }

        public static DatabaseException ConnectionFailed(string details, Exception inner)
            => new DatabaseException($"Database connection failed: {details}", inner);

        public static DatabaseException QueryFailed(string query, Exception inner)
            => new DatabaseException($"Query execution failed: {query}", inner);

        public static DatabaseException TransactionFailed(string operation, Exception inner)
            => new DatabaseException($"Transaction failed during: {operation}", inner);
    }
}
