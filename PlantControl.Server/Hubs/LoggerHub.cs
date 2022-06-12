using Microsoft.AspNetCore.SignalR;
using PlantControl.Models;

namespace PlantControl.Server.Hubs;

public class LoggerHub : Hub<ILoggerHub>
{
    private const string SubscriberGroup = "LoggersSubscribers";
    private const string LoggerGroup = "Loggers";

    //connectionId is key, logger is value
    private static Dictionary<string, Logger> Loggers { get; } = new();

    public override async Task OnConnectedAsync() => await base.OnConnectedAsync();

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        //remove logger from dictionary
        var isLogger = Loggers.TryGetValue(Context.ConnectionId, out var logger);

        if (isLogger)
        {
            Loggers.Remove(Context.ConnectionId);
            await Clients.Group(SubscriberGroup).RemoveLogger(logger.Id);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private static string GetLoggerConnectionId(string loggerId) => Loggers.FirstOrDefault(x => x.Value.Id == loggerId).Key;

    //forward a request to change a specific loggers config
    [HubMethodName("SetConfig")]
    public async Task OnSetConfig(Config config)
    {
        var loggerId = config.Logging.LoggerId;
        var loggerConnectionId = GetLoggerConnectionId(loggerId);
        if (!string.IsNullOrEmpty(loggerConnectionId)) await Clients.Client(loggerConnectionId).SetConfig(config);
    }

    //forward request to get config to all loggers
    [HubMethodName("GetAllConfigs")]
    public async Task OnGetAllConfigs() => await Clients.Group(LoggerGroup).GetConfig();

    //Forward a request for a config to a logger
    [HubMethodName("GetConfig")]
    public async Task OnGetConfig(string id) => await Clients.Client(GetLoggerConnectionId(id)).GetConfig();

    //Forward config from a logger to all subscribers(xamarin app etc.)
    [HubMethodName("SendConfig")]
    public async Task OnSendConfig(Config loggerConfig) => await Clients.Group(SubscriberGroup).ReceiveConfig(loggerConfig);

    //create a new logger and add it to the dictionary. after that, tell all subscribers
    [HubMethodName("ConnectLogger")]
    public async Task<bool> OnConnectLogger(string id)
    {
        //if logger already exists, return false
        if (Loggers.Values.Any(x => x.Id == id)) return false;

        var logger = new Logger {Id = id};
        Loggers[Context.ConnectionId] = logger;

        await Groups.AddToGroupAsync(Context.ConnectionId, LoggerGroup);
        await Clients.Group(SubscriberGroup).NewLogger(logger);

        return true;
    }

    //subscribe to messages about loggers
    [HubMethodName("Subscribe")]
    public async Task OnSubscribe() => await Groups.AddToGroupAsync(Context.ConnectionId, SubscriberGroup);

    //unsubscribe from messages about loggers
    [HubMethodName("Unsubscribe")]
    public async Task OnUnsubscribe() => await Groups.RemoveFromGroupAsync(Context.ConnectionId, SubscriberGroup);

    //return all online loggers
    [HubMethodName("GetOnlineLoggers")]
    public IEnumerable<Logger> OnGetOnlineLoggers() => Loggers.Values;


    //forward a message from a logger to all subscribers
    [HubMethodName("SendLog")]
    public async Task OnSendLog(Log log) => await Clients.Group(SubscriberGroup).ReceiveLog(log);

    [HubMethodName("Calibrate")]
    public async Task OnCalibrate(string calibrationParameter, string loggerId)
    {
        await Clients.Client(GetLoggerConnectionId(loggerId)).Calibrate(calibrationParameter);
        
    }
}