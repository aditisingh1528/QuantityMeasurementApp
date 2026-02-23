using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    
    public interface IQuantityMeasurementService
    {
        bool AreFeetEqual(Feet firstMeasurement, Feet secondMeasurement);
    }
}