namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents the design template for a certificate
/// </summary>
public class CertificateDesign
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Description { get; set; } = string.Empty;
    public string BorderColor { get; set; } = string.Empty; // unique color per certificate
    public string IconSymbol { get; set; } = string.Empty; // emoji or icon representing the achievement
    public string BackgroundColor { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty; // different visual styles
}
