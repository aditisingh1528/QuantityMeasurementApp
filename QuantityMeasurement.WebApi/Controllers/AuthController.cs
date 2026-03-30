using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModel.DTOs;

namespace QuantityMeasurementWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthRequestDTO request)
        {
            try
            {
                _authService.Register(request);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public ActionResult<AuthResponseDTO> Login([FromBody] AuthRequestDTO request)
        {
            try
            {
                var token = _authService.Login(request);
                return Ok(new AuthResponseDTO { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}
