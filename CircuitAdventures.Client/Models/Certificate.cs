namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents a certificate earned by the user at milestones (10, 20, 30, etc.)
/// </summary>
public class Certificate
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty; // certificate title
    public int Level { get; set; } // milestone level (10, 20, 30, etc.)
    public DateTime EarnedDate { get; set; } = DateTime.Now;
    public string UserName { get; set; } = string.Empty;
}
