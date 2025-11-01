namespace CircuitAdventures.Client.Models;

/// <summary>
/// Tracks user's learning streak information
/// </summary>
public class StreakData
{
    public int Current { get; set; } = 0;
    public int Longest { get; set; } = 0;
    public DateTime LastActiveDate { get; set; } = DateTime.Now;
}
