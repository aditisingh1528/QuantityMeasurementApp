using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.DTOs
{
    public class AuthRequestDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
