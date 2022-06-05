using PlantControl.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyHeader();
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowCredentials();
    policyBuilder.SetIsOriginAllowed(_ => true);
});

app.MapHub<LoggerHub>("/hubs/logger");
app.MapHub<UnregisteredLoggerHub>("/hubs/unregisteredLogger");

app.Run();