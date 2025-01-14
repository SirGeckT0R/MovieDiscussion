using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserServiceDataAccess.Handlers;
using UserServiceWebAPI.RoleAuthorization;

namespace UserServiceWebAPI.Extensions
{
    public static class AddAuthExtensions
    {
        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSecretKey"]!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["accessToken"];
                            return Task.CompletedTask;
                        }

                    };
                });


            services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

            services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.AddRequirements(new RoleRequirement("Admin")))
                .AddPolicy("User", policy => policy.AddRequirements(new RoleRequirement("User")));
        }
    }
}

