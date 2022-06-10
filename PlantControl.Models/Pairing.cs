using System;

namespace PlantControl.Models;

public class Pairing
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public Plant Plant { get; set; }
    public string Id { get; set; }
    public Logger Logger { get; set; }
}