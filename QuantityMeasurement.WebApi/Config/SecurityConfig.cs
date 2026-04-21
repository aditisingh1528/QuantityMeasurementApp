using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace QuantityMeasurementWebApi.Config
{
    
    public static class SecurityConfig
    {
        public const string CorsPolicyName = "QuantityMeasurementCorsPolicy";

        public static IServiceCollection AddSecurityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "http://localhost:8080", "http://localhost:5500", "http://127.0.0.1:5500")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var key = configuration["Jwt:Key"];
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"]
                };
            });

            return services;
        }

        public static WebApplication UseSecurityConfig(this WebApplication app)
        {
            app.UseCors(CorsPolicyName);
            return app;
        }
    }
}
