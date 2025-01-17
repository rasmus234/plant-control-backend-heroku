﻿namespace PlantControl.Models;

public class Log
{
    public string _Id { get; set; }
    public DateTime Time { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float Moisture { get; set; }
    public Pairing Pairing { get; set; }
}