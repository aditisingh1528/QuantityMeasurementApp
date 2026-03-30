using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementBusinessLayer.Interfaces
{
    public interface IAuthService
    {
        User Register(AuthRequestDTO request);
        string Login(AuthRequestDTO request);
    }
}
