using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModel.DTOs;

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
            return service.Compare(q1, q2);
        }

        public QuantityDTO Convert(QuantityDTO q, string target)
        {
            return service.Convert(q, target);
        }

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
        {
            return service.Add(q1, q2);
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
        {
            return service.Subtract(q1, q2);
        }

        public double Divide(QuantityDTO q1, QuantityDTO q2)
        {
            return service.Divide(q1, q2);
        }
    }
}