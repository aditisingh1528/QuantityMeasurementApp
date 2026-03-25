namespace QuantityMeasurementWebApi.Config
{
    /// 
    /// UC17 Step 9: SecurityConfig – basic security setup.
    ///
    /// All requests are allowed for development and testing purposes.
    /// CORS is configured permissively so the API can be called from a
    /// browser front-end on a different port during development.
    /// Lock down origins before deploying to production.
    /// 
    public static class SecurityConfig
    {
        // name of the CORS policy – referenced in Program.cs
        public const string CorsPolicyName = "QuantityMeasurementCorsPolicy";

        /// 
        /// Registers the CORS policy on the service collection.
        /// Allows all origins, headers, and methods for dev/testing.
        /// 
        public static IServiceCollection AddSecurityConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }

        /// 
        /// Adds the CORS middleware to the pipeline.
        /// Called in Program.cs before UseAuthorization.
        /// 
        public static WebApplication UseSecurityConfig(this WebApplication app)
        {
            app.UseCors(CorsPolicyName);
            return app;
        }
    }
}
