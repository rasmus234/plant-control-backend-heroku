using PlantControl.Models;

namespace PlantControl.Server.Hubs;

public interface IUnregisteredLoggerHub
{
    Task NewUnregisteredLogger(UnregisteredLogger logger);
    Task RemoveUnregisteredLogger(string id);
}