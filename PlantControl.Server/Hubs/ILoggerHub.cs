using PlantControl.Models;

namespace PlantControl.Server.Hubs;

public interface ILoggerHub
{
    Task GetConfig();

    Task SetConfig(Config config);
    Task SetPairingId(string id);
    Task ReceiveConfig(Config config);

    Task NewLogger(Logger logger);
    Task RemoveLogger(string id);

    Task ReceiveLog(Log log);
    
    Task Calibrate(string parameter);
}