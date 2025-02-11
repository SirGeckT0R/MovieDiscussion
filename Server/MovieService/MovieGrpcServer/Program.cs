using MovieGrpcServer;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder);

var app = builder.Build();
startup.Configure(app, app.Environment);

await app.RunAsync();
