using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementBusinessLayer.Services
{
    /// <summary>
    /// UC16: QuantityMeasurementService(IQuantityMeasurementRepository) — dependency-injected constructor
    ///       that wires the repository into the service for persistence.
    /// </summary>
    public class QuantityMeasurementService : QuantityMeasurementServiceImpl
    {
        // UC15-compatible: no-arg constructor
        public QuantityMeasurementService() : base()
        {
        }

        // UC16: accepts injected repository (database or cache)
        public QuantityMeasurementService(IQuantityMeasurementRepository repository)
            : base(repository)
        {
        }
    }
}
