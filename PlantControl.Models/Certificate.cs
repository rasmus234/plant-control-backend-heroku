namespace PlantControl.Models;

public class Certificate
{
    
    public string _Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public Plant Plant { get; set; }
}