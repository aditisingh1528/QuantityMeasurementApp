using System;

namespace QuantityMeasurementModel.Exceptions
{
    public class QuantityMeasurementException : Exception
    {
        public QuantityMeasurementException(string message)
            : base(message)
        {
        }
    }
}