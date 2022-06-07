namespace PlantControl.Models;

public class LoggerConfig
{
    public Logging Logging { get; set; }
    public Air Air { get; set; }
    public Soil Soil { get; set; }
}

public record class Logging(string Id, bool Active, string HubUrl, string RestUrl)
{
    public string Id { get; } = Id;
    public bool Active { get; } = Active;
    public string HubUrl { get; } = HubUrl;
    public string RestUrl { get; } = RestUrl;
}

public record class Air(float MinHumid, float MaxHumid, float MinTemp, float MaxTemp)
{
    public float MinHumid { get; } = MinHumid;
    public float MaxHumid { get; } = MaxHumid;
    public float MinTemp { get; } = MinTemp;
    public float MaxTemp { get; } = MaxTemp;
}

public record class Soil(float Moist, float Dry)
{
    public float Moist { get; } = Moist;
    public float Dry { get; } = Dry;
}