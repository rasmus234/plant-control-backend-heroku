﻿using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using PlantControl.Models;

namespace PlantControl.Server.Hubs;

public class LoggerHub : Hub<ILoggerHub>
{
    private const string SubscriberGroup = "LoggersSubscribers";
    private const string LoggerGroup = "Loggers";
    
    //connectionId is key, logger is value
    public static Dictionary<string, Logger> Loggers { get; } = new();

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        //remove logger from dictionary
        var isLogger = Loggers.TryGetValue(Context.ConnectionId, out var logger);
        if (isLogger)
        {
            Loggers.Remove(Context.ConnectionId);
            await Clients.Group(SubscriberGroup).RemoveLogger(logger._Id);
        }

        await base.OnDisconnectedAsync(exception);
    }

    //forward a request to change a specific loggers config
    [HubMethodName("SetConfig")]
    public async Task OnSetConfig(LoggerConfig loggerConfig)
    {
        var loggerId = loggerConfig.Logging.LoggerId;
        var loggerConnectionId = Loggers.FirstOrDefault(x => x.Value._Id == loggerId).Key;
        if (!string.IsNullOrEmpty(loggerConnectionId))
        {
            await Clients.Client(loggerConnectionId).SetConfig(loggerConfig);
        }
    }
    //forward request to get config to all loggers
    [HubMethodName("GetAllConfigs")]
    public async Task OnGetAllConfigs()
    {
        await Clients.Group(LoggerGroup).GetConfig();
    }

    //Forward a request for a config to a logger
    [HubMethodName("GetConfig")]
    public async Task OnGetConfig(string id)
    {
        
        var loggerConnectionId = Loggers.FirstOrDefault(x => x.Value._Id == id).Key;
        await Clients.Client(loggerConnectionId).GetConfig();
    }

    //Forward a loggerconfig from a logger to all subscribers(xamarin app etc.)
    [HubMethodName("SendConfig")]
    public async Task OnSendConfig(LoggerConfig loggerConfig)
    {
        await Clients.Group(SubscriberGroup).ReceiveConfig(loggerConfig);
    }

    //create a new logger and add it to the dictionary. after that, tell all subscribers
    [HubMethodName("ConnectLogger")]
    public async Task<bool> OnConnectLogger(string id)
    {
        //if logger already exists, return false
        if (Loggers.Values.Any(x => x._Id == id))
        {
            return false;
        }

        var logger = new Logger {_Id = id, LoginTime = DateTime.Now};
        Loggers[Context.ConnectionId] = logger;
        await Groups.AddToGroupAsync(Context.ConnectionId, LoggerGroup);
        await Clients.Group(SubscriberGroup).NewLogger(logger);
        return true;
    }

    //subscribe to messages about loggers
    [HubMethodName("Subscribe")]
    public async Task OnSubscribe()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, SubscriberGroup);
    }

    //unsubscribe from messages about loggers
    [HubMethodName("Unsubscribe")]
    public async Task OnUnsubscribe()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, SubscriberGroup);
    }

    //return all online loggers
    [HubMethodName("GetOnlineLoggers")]
    public IEnumerable<Logger> OnGetOnlineLoggers()
    {
        return Loggers.Values;
    }

    
    //forward a message from a logger to all subscribers
    [HubMethodName("SendLog")]
    public async Task OnSendLog(Log log)
    {
        await Clients.Group(SubscriberGroup).ReceiveLog(log);
    }
}