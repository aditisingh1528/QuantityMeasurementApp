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
        {
            var result = service.Compare(q1, q2);
            return result.ResultString == "true";
        }

        public QuantityDTO Convert(QuantityDTO q, string target)
        {
            var targetDTO = new QuantityDTO(0.0, target, q.MeasurementType);
            var result    = service.Convert(q, targetDTO);
            return new QuantityDTO(result.ResultValue, result.ResultUnit ?? target);
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            var result = service.Add(q1, q2);
            return new QuantityDTO(result.ResultValue, result.ResultUnit ?? q1.Unit);
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            var result = service.Subtract(q1, q2);
            return new QuantityDTO(result.ResultValue, result.ResultUnit ?? q1.Unit);
        }

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            var result = service.Divide(q1, q2);
            return result.ResultValue;
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
            => service.GetAllMeasurements();

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
            => service.GetMeasurementsByOperation(operationType);

        public string GetPoolStatistics()
            => service.GetPoolStatistics();
    }
}
