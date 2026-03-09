using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    public interface IQuantityMeasurementService
    {
        bool AreLengthsEqual(Length first, Length second);
        bool AreWeightsEqual(Weight first, Weight second);
    }
}