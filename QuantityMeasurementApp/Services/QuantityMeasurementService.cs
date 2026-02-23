using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        public bool AreLengthsEqual(Length first, Length second)
        {
            if (first is null || second is null)
                throw new ArgumentNullException("Length cannot be null.");

            return first.Equals(second);
        }
    }
}