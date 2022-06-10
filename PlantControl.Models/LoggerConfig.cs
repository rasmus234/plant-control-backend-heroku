namespace PlantControl.Models;

public class LoggerConfig
{
    public Logging Logging { get; set; }
    public Air Air { get; set; }
    public Soil Soil { get; set; }
}

public record class Logging(string LoggerId, bool Active, string SocketUrl, string RestUrl, string PairingId)
{
    public string LoggerId { get; } = LoggerId;
    public bool Active { get; } = Active;
    public string SocketUrl { get; } = SocketUrl;
    public string RestUrl { get; } = RestUrl;
    public string PairingId { get; } = PairingId;
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