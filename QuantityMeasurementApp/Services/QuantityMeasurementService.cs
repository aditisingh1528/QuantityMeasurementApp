using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        public bool AreFeetEqual(Feet firstMeasurement, Feet secondMeasurement)
        {
            
            if (firstMeasurement is null || secondMeasurement is null)
                throw new QuantityMeasurementException("Measurement values cannot be null.");

            return firstMeasurement.Equals(secondMeasurement);
        }
    }
}