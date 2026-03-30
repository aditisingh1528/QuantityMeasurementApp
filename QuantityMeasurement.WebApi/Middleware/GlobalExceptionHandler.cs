using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuantityMeasurementModel.Exceptions;

namespace QuantityMeasurementWebApi.Middleware
{
    // GlobalExceptionHandler – centralised error handling for all controllers. 
    public class GlobalExceptionHandler : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            ErrorResponse error;

            if (context.Exception is QuantityMeasurementException qex)
            {
                _logger.LogWarning("Handling QuantityMeasurementException: {Message} for path: {Path}",
                    qex.Message, context.HttpContext.Request.Path);

                error = new ErrorResponse
                {
                    Timestamp = DateTime.UtcNow,
                    Status    = 400,
                    Error     = "Quantity Measurement Error",
                    Message   = qex.Message,
                    Path      = context.HttpContext.Request.Path
                };

                context.Result = new BadRequestObjectResult(error);
            }
            else if (context.Exception is ValidationException vex)
            {
                _logger.LogWarning("Handling ValidationException: {Message} for path: {Path}",
                    vex.Message, context.HttpContext.Request.Path);

                error = new ErrorResponse
                {
                    Timestamp = DateTime.UtcNow,
                    Status    = 400,
                    Error     = "Validation Error",
                    Message   = vex.Message,
                    Path      = context.HttpContext.Request.Path
                };

                context.Result = new BadRequestObjectResult(error);
            }
            else if (context.Exception is UserAlreadyExistsException uex)
            {
                _logger.LogWarning("Handling UserAlreadyExistsException: {Message} for path: {Path}",
                    uex.Message, context.HttpContext.Request.Path);

                error = new ErrorResponse
                {
                    Timestamp = DateTime.UtcNow,
                    Status    = 409,
                    Error     = "Conflict",
                    Message   = uex.Message,
                    Path      = context.HttpContext.Request.Path
                };

                context.Result = new ObjectResult(error) { StatusCode = 409 };
            }
            else
            {
                _logger.LogError(context.Exception,
                    "Handling global exception for path: {Path}", context.HttpContext.Request.Path);

                error = new ErrorResponse
                {
                    Timestamp = DateTime.UtcNow,
                    Status    = 500,
                    Error     = "Internal Server Error",
                    Message   = context.Exception.Message,
                    Path      = context.HttpContext.Request.Path
                };

                context.Result = new ObjectResult(error) { StatusCode = 500 };
            }

            // mark the exception as handled so ASP.NET does not do its own processing
            context.ExceptionHandled = true;
        }
    }

    // Standard error response body returned by the exception handler
    public class ErrorResponse
    {
        public DateTime Timestamp { get; set; }
        public int      Status    { get; set; }
        public string   Error     { get; set; } = string.Empty;
        public string   Message   { get; set; } = string.Empty;
        public string   Path      { get; set; } = string.Empty;
    }
}
