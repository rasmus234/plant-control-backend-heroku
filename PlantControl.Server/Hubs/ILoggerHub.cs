using PlantControl.Models;

namespace PlantControl.Server.Hubs;

public interface ILoggerHub
{
    Task GetConfig();
    
    Task SetConfig(LoggerConfig loggerConfig);
    Task ReceiveConfig(LoggerConfig loggerConfig);
    
    Task NewLogger(Logger logger);
    Task RemoveLogger(string id);
    
    Task ReceiveLog(Log log);
}