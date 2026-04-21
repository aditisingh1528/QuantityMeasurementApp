using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/quantities")]
    [Produces("application/json")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;
        private readonly ILogger<QuantityMeasurementController> _logger;

        public QuantityMeasurementController(
            IQuantityMeasurementService service,
            ILogger<QuantityMeasurementController> logger)
        {
            _service = service;
            _logger  = logger;
        }

        //  Public dashboard endpoints — no login required z
        // Com  pare two quantities
        [AllowAnonymous]
        [HttpPost("compare")]
        public ActionResult<QuantityMeasurementDTO> Compare([FromBody] TwoOperandRequestDTO request)
        {
            var result = _service.Compare(request.This!, request.That!);
            return Ok(result);
        }

        // Convert a quantity to a different unit of the same type
        [AllowAnonymous]
        [HttpPost("convert")]
        public ActionResult<QuantityMeasurementDTO> Convert([FromBody] ConvertRequestDTO request)
        {
            // Build a target QuantityDTO containing only the desired unit
            var targetDTO = new QuantityDTO(0, request.ToUnit!);
            var result    = _service.Convert(request.From!, targetDTO);
            return Ok(result);
        }

        // Add two quantities- result expressed in the unit of the first operand
        [AllowAnonymous]
        [HttpPost("add")]
        public ActionResult<QuantityMeasurementDTO> Add([FromBody] TwoOperandRequestDTO request)
        {
            var result = _service.Add(request.This!, request.That!);
            return Ok(result);
        }

        // Add two quantities- result expressed in the specified target unit
        [AllowAnonymous]
        [HttpPost("add-with-target-unit")]
        public ActionResult<QuantityMeasurementDTO> AddWithTargetUnit(
            [FromBody] ArithmeticWithTargetRequestDTO request)
        {
            var targetDTO = new QuantityDTO(0, request.TargetUnit!);
            var result    = _service.Add(request.This!, request.That!, targetDTO);
            return Ok(result);
        }

        // Subtract two quantities- result expressed in the unit of the first operand.
        [AllowAnonymous]
        [HttpPost("subtract")]
        public ActionResult<QuantityMeasurementDTO> Subtract([FromBody] TwoOperandRequestDTO request)
        {
            var result = _service.Subtract(request.This!, request.That!);
            return Ok(result);
        }

        // Subtract two quantities- result expressed in the specified target unit.
        [AllowAnonymous]
        [HttpPost("subtract-with-target-unit")]
        public ActionResult<QuantityMeasurementDTO> SubtractWithTargetUnit(
            [FromBody] ArithmeticWithTargetRequestDTO request)
        {
            var targetDTO = new QuantityDTO(0, request.TargetUnit!);
            var result    = _service.Subtract(request.This!, request.That!, targetDTO);
            return Ok(result);
        }

        // Divide two quantities- returns a dimensionless ratio
        [AllowAnonymous]
        [HttpPost("divide")]
        public ActionResult<QuantityMeasurementDTO> Divide([FromBody] TwoOperandRequestDTO request)
        {
            var result = _service.Divide(request.This!, request.That!);
            return Ok(result);
        }

        //  Protected history endpoints — login required 

        // Returns all saved operations matching the given operation name (COMPARE, ADD, etc)
        [Authorize]
        [HttpGet("history/operation/{operation}")]
        public ActionResult<List<QuantityMeasurementDTO>> GetByOperation(string operation)
        {
            return Ok(_service.GetOperationHistory(operation));
        }

        // Returns all saved operations for a given measurement type (LengthUnit, etc)
        [Authorize]
        [HttpGet("history/type/{type}")]
        public ActionResult<List<QuantityMeasurementDTO>> GetByType(string type)
        {
            return Ok(_service.GetMeasurementsByType(type));
        }

        // Returns the count of successful (non-error) operations of a given type
        [Authorize]
        [HttpGet("count/{operation}")]
        public ActionResult<long> GetCount(string operation)
        {
            return Ok(_service.GetOperationCount(operation));
        }

        // Returns all operations that resulted in an error
        [Authorize]
        [HttpGet("history/errored")]
        public ActionResult<List<QuantityMeasurementDTO>> GetErrored()
        {
            return Ok(_service.GetErrorHistory());
        }
    }
}