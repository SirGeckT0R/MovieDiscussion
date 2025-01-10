using MMLib.Ocelot.Provider.AppConfiguration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            //builder.Services.AddOcelot(builder.Configuration).AddAppConfiguration();
            //builder.Services.AddSwaggerForOcelot(builder.Configuration);

        }
        public void Configure(WebApplication app, IWebHostEnvironment env, string[] args)
        {
            //app.UseSwaggerForOcelotUI(opt =>
            //{
            //    opt.PathToSwaggerGenerator = "/swagger/docs";
            //});
            //app.UseOcelot().Wait();

        }
    }
}
