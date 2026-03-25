using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Exceptions;
using Xunit;

namespace QuantityMeasurement.Tests
{
    public class QuantityMeasurementControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IQuantityMeasurementService> _mockService;

        public QuantityMeasurementControllerTest(WebApplicationFactory<Program> factory)
        {
            _mockService = new Mock<IQuantityMeasurementService>();

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IQuantityMeasurementService));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddSingleton(_mockService.Object);
                });
            });
        }

        //  helpers 

        private static TwoOperandRequestDTO TwoOp(
            double v1, string u1, string mt1,
            double v2, string u2) =>
            new()
            {
                This = new QuantityDTO(v1, u1, mt1),
                That = new QuantityDTO(v2, u2, mt1)
            };

        private static QuantityMeasurementDTO MakeResult(
            string operation, double resultValue, string? resultString = null) =>
            new()
            {
                Operation    = operation,
                ResultValue  = resultValue,
                ResultString = resultString,
                IsError      = false
            };

        //  compare 

        [Fact]
        public async Task TestCompareQuantities_Success()
        {
            var mockResult = MakeResult("COMPARE", 1.0, "true");
            _mockService
                .Setup(s => s.Compare(It.IsAny<QuantityDTO>(), It.IsAny<QuantityDTO>()))
                .Returns(mockResult);

            var client   = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/v1/quantities/compare",
                TwoOp(1.0, "FEET", "LengthUnit", 12.0, "INCHES"));
            var result   = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("true", result!.ResultString);
        }

        //  add 

        [Fact]
        public async Task TestAddQuantities_Success()
        {
            var mockResult = MakeResult("ADD", 2.0);
            mockResult.ResultUnit = "FEET";

            _mockService
                .Setup(s => s.Add(It.IsAny<QuantityDTO>(), It.IsAny<QuantityDTO>()))
                .Returns(mockResult);

            var client   = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/v1/quantities/add",
                TwoOp(1.0, "FEET", "LengthUnit", 12.0, "INCHES"));
            var result   = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2.0, result!.ResultValue);
        }

        //  history / count 

        [Fact]
        public async Task TestGetOperationHistory_Success()
        {
            _mockService
                .Setup(s => s.GetOperationHistory("COMPARE"))
                .Returns(new List<QuantityMeasurementDTO>());

            var client   = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/quantities/history/operation/COMPARE");
            var result   = await response.Content.ReadFromJsonAsync<List<QuantityMeasurementDTO>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Empty(result!);
        }

        [Fact]
        public async Task TestGetOperationCount_Success()
        {
            _mockService
                .Setup(s => s.GetOperationCount("COMPARE"))
                .Returns(0L);

            var client   = _factory.CreateClient();
            var response = await client.GetAsync("/api/v1/quantities/count/COMPARE");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("0", await response.Content.ReadAsStringAsync());
        }

        //  error handling 

        [Fact]
        public async Task TestCompareQuantities_InvalidUnit_Returns400()
        {
            _mockService
                .Setup(s => s.Compare(It.IsAny<QuantityDTO>(), It.IsAny<QuantityDTO>()))
                .Throws(new QuantityMeasurementException("Invalid unit: FOOT"));

            var client   = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/v1/quantities/compare",
                TwoOp(1.0, "FOOT", "LengthUnit", 12.0, "INCHE"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
