using QuantityMeasurementRepository.EFCore;
using QuantityMeasurementRepository.Interface;

namespace QuantityMeasurementBusinessLayer.Services
{
    public class QuantityMeasurementService : QuantityMeasurementServiceImpl
    {
        // UC15-compatible: no-arg constructor
        public QuantityMeasurementService() : base()
        {
        }

        // UC16: accepts legacy cache/database repository
        public QuantityMeasurementService(IQuantityMeasurementRepository repository)
            : base(repository)
        {
        }

        // UC17: accepts EF Core repository (used by Web API)
        public QuantityMeasurementService(IQuantityMeasurementJpaRepository jpaRepository)
            : base(jpaRepository)
        {
        }
    }
}
