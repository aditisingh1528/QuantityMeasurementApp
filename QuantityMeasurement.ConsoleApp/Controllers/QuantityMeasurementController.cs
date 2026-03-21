using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementConsoleApp.Controllers
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            this.service = service;
        }
        public bool Compare(QuantityDTO q1, QuantityDTO q2)
            => service.Compare(q1, q2);

        public QuantityDTO Convert(QuantityDTO q, string target)
            => service.Convert(q, target);

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
            => service.Add(q1, q2);

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
            => service.Subtract(q1, q2);

        public double Divide(QuantityDTO q1, QuantityDTO q2)
            => service.Divide(q1, q2);

        public List<QuantityMeasurementEntity> GetAllMeasurements()
            => service.GetAllMeasurements();

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
            => service.GetMeasurementsByOperation(operationType);

        public string GetPoolStatistics()
            => service.GetPoolStatistics();
    }
}
