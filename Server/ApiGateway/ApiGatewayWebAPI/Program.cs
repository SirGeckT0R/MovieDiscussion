using ApiGatewayWebAPI;

var builder = WebApplication.CreateBuilder();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder);
builder.Services.AddControllers();

var app = builder.Build();
startup.Configure(app, app.Environment);
app.MapControllers();
await app.RunAsync();
