using Microsoft.EntityFrameworkCore;
using PlantControl.Server;
using PlantControl.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSignalR();
builder.Services.AddDbContext<PlantControlContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("PlantControlDatabase")!);
});

var app = builder.Build();

app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyHeader();
    policyBuilder.AllowAnyMethod();
    policyBuilder.SetIsOriginAllowed(_ => true);
});

app.MapGet("/", () => "Hello World!");

app.MapHub<LoggerHub>("/hubs/logger");

app.Run();