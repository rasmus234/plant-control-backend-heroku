namespace PlantControl.Models;

public class LoggerConfig
{
    public Logging Logging { get; set; }
    public Air Air { get; set; }
    public Soil Soil { get; set; }
}




public record class Logging(string Id, bool Active, string HubUrl, string RestUrl);

public record class Air(float MinHumid, float MaxHumid, float MinTemp, float MaxTemp);

public record class Soil(float Moist, float Dry);