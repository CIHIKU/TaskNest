using AuthService.Configuration;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices.Configure(builder.Services);

var app = builder.Build();

ConfigureApplication.Configure(app);

app.Run();