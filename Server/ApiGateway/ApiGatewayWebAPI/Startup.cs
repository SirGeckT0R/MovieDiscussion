using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MMLib.Ocelot.Provider.AppConfiguration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Text;
using UserServiceWebAPI.RoleAuthorization;

namespace ApiGatewayWebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("routes.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration).AddAppConfiguration();
            builder.Services.AddSwaggerForOcelot(Configuration);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSecretKey"]!))
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


            builder.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.AddRequirements(new RoleRequirement("Admin")))
                .AddPolicy("User", policy => policy.AddRequirements(new RoleRequirement("User")));
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
            });

            app.UseOcelot().GetAwaiter().GetResult();
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
