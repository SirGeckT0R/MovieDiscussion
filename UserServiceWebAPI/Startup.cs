namespace UserServiceWebAPI
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
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
            }
            public void Configure(WebApplication app, IWebHostEnvironment env, string[] args)
            {
                if(env.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseAuthorization();
            }
        }
}

