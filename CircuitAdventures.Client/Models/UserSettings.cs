namespace CircuitAdventures.Client.Models;

/// <summary>
/// User preferences and settings
/// </summary>
public class UserSettings
{
    public bool SoundEnabled { get; set; } = true;
    public string TextSize { get; set; } = "medium"; // small, medium, large
}
