using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using PlantControl.Models;

namespace PlantControl.Server.Hubs;

public class UnregisteredLoggerHub : Hub<IUnregisteredLoggerHub>
{
    private const string SubscriberGroup = "UnregisteredLoggersSubscribers";
    public static Dictionary<string, UnregisteredLogger> UnregisteredLoggers { get; } = new();


    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        //remove logger from dictionary
        var exists = UnregisteredLoggers.Remove(Context.ConnectionId);
        //if logger was removed, tell all subscribers
        if (exists)
        {
            await Clients.Group(SubscriberGroup).RemoveUnregisteredLogger(Context.ConnectionId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<IEnumerable<UnregisteredLogger>> GetUnregisteredLoggers()
    {
        return UnregisteredLoggers.Values;
    }
    //create a new unregistered logger and add it to the dictionary. after that, tell all subscribers
    public async Task ConnectUnregisteredLogger()
    {
        var logger = new UnregisteredLogger {Id = Context.ConnectionId, FirstSeen = DateTime.Now};
        UnregisteredLoggers[Context.ConnectionId] = logger;
        await Clients.Group(SubscriberGroup).NewUnregisteredLogger(logger);
    }

    //subscribe to messages about new and removed unregistered loggers
    public async Task Subscribe()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, SubscriberGroup);
    }
    
    //register a unregistered logger and remove it from the dictionary. after that, tell all subscribers
    public async Task<Logger?> RegisterLogger(UnregisteredLogger logger)
    {
        if (!UnregisteredLoggers.ContainsKey(logger.Id)) return null;
        using var client = new HttpClient();
        var json = JsonSerializer.Serialize(logger, new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var content = new StringContent(json);
        var response = await client.PostAsync("http://localhost:3000/loggers", content);
        var loggerJson = await response.Content.ReadAsStringAsync();
        var registeredLogger = JsonSerializer.Deserialize<Logger>(loggerJson);
        return registeredLogger;

    }
}