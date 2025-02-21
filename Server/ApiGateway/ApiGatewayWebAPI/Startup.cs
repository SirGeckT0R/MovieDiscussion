using ApiGatewayWebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

namespace ApiGatewayWebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.WithOrigins(Configuration["FrontendUrl"]!)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            builder.Configuration.AddOcelotWithSwaggerSupport((options) =>
            {
                options.Folder = "OcelotConfiguration";
                options.FileOfSwaggerEndPoints = "ocelot.swagger";
            });

            builder.Services.AddOcelot();
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

            builder.Services.AddControllers();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseSwaggerForOcelotUI(opt =>
                {
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                });
            }

            app.UseCors("AllowAll");

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.SameAsRequest,
            });

            app.UseWebSockets();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExtractAccountIdMiddleware>();

            app.UseOcelot().GetAwaiter().GetResult();

            app.MapControllers();
        }
    }
}
