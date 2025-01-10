using DiscussionServiceWebAPI;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder);
var app = builder.Build();

startup.Configure(app, app.Environment, args);

app.MapControllers();

app.Run();
