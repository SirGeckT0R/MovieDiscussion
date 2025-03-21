using UserServiceWebAPI;

var builder = WebApplication.CreateBuilder();

var startup = new Startup(builder.Configuration, builder.Environment);

startup.ConfigureServices(builder);
var app = builder.Build();

startup.Configure(app);

await app.RunAsync();

public partial class Program { }
