using ApiGateway;

var builder = WebApplication.CreateBuilder();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder);

var app = builder.Build();
startup.Configure(app);

await app.RunAsync();
