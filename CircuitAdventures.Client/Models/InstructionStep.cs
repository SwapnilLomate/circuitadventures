namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents a single instruction step in a level
/// </summary>
public class InstructionStep
{
    public int StepNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DiagramUrl { get; set; } = string.Empty; // SVG diagram for this step
    public string? Tip { get; set; }
    public string? Warning { get; set; }
}
