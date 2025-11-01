namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents a component needed for a level
/// </summary>
public class ComponentItem
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
