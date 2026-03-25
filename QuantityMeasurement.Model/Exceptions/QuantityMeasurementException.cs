namespace QuantityMeasurementModel.Exceptions
{
    public class QuantityMeasurementException : Exception
    {
        public QuantityMeasurementException(string message)
            : base(message) { }

        // UC16: added inner-exception overload needed by DatabaseException
        public QuantityMeasurementException(string message, Exception inner)
            : base(message, inner) { }
    }
}
