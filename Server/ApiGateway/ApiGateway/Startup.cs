using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            //builder.Configuration.AddJsonFile("routes.json", optional: false, reloadOnChange: true);
            //builder.Services.AddOcelot(builder.Configuration);
        }

        public void Configure(WebApplication app)
        {
            //app.UseOcelot().GetAwaiter().GetResult();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
