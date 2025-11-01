namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents an optional challenge mode for a level
/// </summary>
public class Challenge
{
    public string Description { get; set; } = string.Empty;
    public string SuccessCriteria { get; set; } = string.Empty;
}
