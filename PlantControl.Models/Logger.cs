﻿namespace PlantControl.Models;

public class Logger
{
    
    public string _Id { get; set; }
    public string Name { get; set; }
    public DateTime? LoginTime { get; set; }
    public bool IsPaired { get; set; } = false;
}