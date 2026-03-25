using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using QuantityMeasurementModel.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace QuantityMeasurement.Tests
{
    [Collection("Integration")]
    public class QuantityMeasurementApplicationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        private const string BaseUrl = "/api/v1/quantities";

        public QuantityMeasurementApplicationTests(
            WebApplicationFactory<Program> factory,
            ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
            _output.WriteLine("Test server running.");
        }

        //  request body helpers 

        private static TwoOperandRequestDTO TwoOp(
            double v1, string u1, string mt1,
            double v2, string u2, string? mt2 = null) =>
            new()
            {
                This = new QuantityDTO(v1, u1, mt1),
                That = new QuantityDTO(v2, u2, mt2 ?? mt1)
            };

        private static ConvertRequestDTO ConvertOp(
            double v, string fromUnit, string toUnit) =>
            new()
            {
                From  = new QuantityDTO(v, fromUnit),
                ToUnit = toUnit
            };

        private static ArithmeticWithTargetRequestDTO WithTarget(
            double v1, string u1, string mt1,
            double v2, string u2,
            string targetUnit) =>
            new()
            {
                This       = new QuantityDTO(v1, u1, mt1),
                That       = new QuantityDTO(v2, u2, mt1),
                TargetUnit = targetUnit
            };

        //  compare 

        [Fact]
        public async Task TestCompare_FootEqualsInches_ResultIsTrue()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/compare",
                TwoOp(1.0, "FEET", "LengthUnit", 12.0, "INCHES"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal("true", result!.ResultString);
        }

        [Fact]
        public async Task TestCompare_FootNotEqualOneInch_ResultIsFalse()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/compare",
                TwoOp(1.0, "FEET", "LengthUnit", 1.0, "INCHES"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("false", result!.ResultString);
        }

        [Fact]
        public async Task TestCompare_GallonEqualsLitres_ResultIsTrue()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/compare",
                TwoOp(1.0, "GALLON", "VolumeUnit", 3.785, "LITRE"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("true", result!.ResultString);
        }

        [Fact]
        public async Task TestCompare_FahrenheitEqualsCelsius_ResultIsTrue()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/compare",
                TwoOp(212.0, "FAHRENHEIT", "TemperatureUnit", 100.0, "CELSIUS"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("true", result!.ResultString);
        }

        //  convert 

        [Fact]
        public async Task TestConvert_CelsiusToFahrenheit_100To212()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/convert",
                ConvertOp(100.0, "CELSIUS", "FAHRENHEIT"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(212.0, result!.ResultValue, precision: 1);
        }

        //  add 

        [Fact]
        public async Task TestAdd_GallonAndLitres_ResultIs2Gallons()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/add",
                TwoOp(1.0, "GALLON", "VolumeUnit", 3.785, "LITRE"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2.0, result!.ResultValue, precision: 1);
        }

        [Fact]
        public async Task TestAdd_WithTargetUnit_FootAndInchesTo24Inches()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/add-with-target-unit",
                WithTarget(1.0, "FEET", "LengthUnit", 12.0, "INCHES", "INCHES"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(24.0, result!.ResultValue, precision: 1);
        }

        //  subtract 

        [Fact]
        public async Task TestSubtract_FeetMinusInches_ResultIs1Foot()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/subtract",
                TwoOp(2.0, "FEET", "LengthUnit", 12.0, "INCHES"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1.0, result!.ResultValue, precision: 1);
        }

        [Fact]
        public async Task TestSubtract_WithTargetUnit_FeetAndInchesTo12Inches()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/subtract-with-target-unit",
                WithTarget(2.0, "FEET", "LengthUnit", 12.0, "INCHES", "INCHES"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(12.0, result!.ResultValue, precision: 1);
        }

        //  divide 

        [Fact]
        public async Task TestDivide_YardByFoot_ResultIs3()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/divide",
                TwoOp(1.0, "YARDS", "LengthUnit", 1.0, "FEET"));
            var result = await response.Content.ReadFromJsonAsync<QuantityMeasurementDTO>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3.0, result!.ResultValue, precision: 1);
        }

        [Fact]
        public async Task TestDivide_ByZero_Returns500()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/divide",
                TwoOp(1.0, "YARDS", "LengthUnit", 0.0, "FEET"));

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        //  history / count 

        [Fact]
        public async Task TestGetOperationHistory_Convert_ReturnsNonEmpty()
        {
            await _client.PostAsJsonAsync($"{BaseUrl}/convert",
                ConvertOp(100.0, "CELSIUS", "FAHRENHEIT"));

            var response = await _client.GetAsync($"{BaseUrl}/history/operation/CONVERT");
            var result   = await response.Content.ReadFromJsonAsync<List<QuantityMeasurementDTO>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(result!);
        }

        [Fact]
        public async Task TestGetHistoryByType_TemperatureUnit_ReturnsNonEmpty()
        {
            await _client.PostAsJsonAsync($"{BaseUrl}/compare",
                TwoOp(212.0, "FAHRENHEIT", "TemperatureUnit", 100.0, "CELSIUS"));

            var response = await _client.GetAsync($"{BaseUrl}/history/type/TemperatureUnit");
            var result   = await response.Content.ReadFromJsonAsync<List<QuantityMeasurementDTO>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(result!);
        }

        [Fact]
        public async Task TestGetOperationCount_Divide_GreaterThanZero()
        {
            await _client.PostAsJsonAsync($"{BaseUrl}/divide",
                TwoOp(1.0, "YARDS", "LengthUnit", 1.0, "FEET"));

            var response = await _client.GetAsync($"{BaseUrl}/count/DIVIDE");
            var count    = long.Parse(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(count > 0);
        }

        //  validation failures 

        [Fact]
        public async Task TestCompare_InvalidUnit_Returns400()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/compare",
                TwoOp(1.0, "FOOT", "LengthUnit", 12.0, "INCHES"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task TestAdd_IncompatibleTypes_Returns400()
        {
            var response = await _client.PostAsJsonAsync($"{BaseUrl}/add",
                TwoOp(1.0, "FEET", "LengthUnit", 1.0, "KILOGRAM", "WeightUnit"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cannot perform arithmetic between different measurement categories", content);
        }

        [Fact]
        public async Task TestDivide_ByZeroFoot_ErrorHistoryContainsRecord()
        {
            await _client.PostAsJsonAsync($"{BaseUrl}/divide",
                TwoOp(1.0, "YARDS", "LengthUnit", 0.0, "FEET"));

            var errorResponse = await _client.GetAsync($"{BaseUrl}/history/errored");
            var errors = await errorResponse.Content.ReadFromJsonAsync<List<QuantityMeasurementDTO>>();

            Assert.Equal(HttpStatusCode.OK, errorResponse.StatusCode);
            Assert.NotEmpty(errors!);
            Assert.Contains(errors!, e => e.IsError);
        }
    }
}
