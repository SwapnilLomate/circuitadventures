namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents an achievement badge
/// </summary>
public class Badge
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string RequirementDescription { get; set; } = string.Empty;
}
