namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents all user data stored in localStorage
/// </summary>
public class UserData
{
    public string Name { get; set; } = string.Empty;
    public string? Avatar { get; set; } // optional avatar choice
    public DateTime FirstVisit { get; set; } = DateTime.Now;
    public DateTime LastVisit { get; set; } = DateTime.Now;
    public int CurrentLevel { get; set; } = 1; // 1-100
    public List<int> CompletedLevels { get; set; } = new();
    public Dictionary<int, int> StarsEarned { get; set; } = new(); // level ID -> stars (1-3)
    public List<Certificate> Certificates { get; set; } = new();
    public int TotalTimeSpent { get; set; } // minutes
    public StreakData Streak { get; set; } = new();
    public List<string> Badges { get; set; } = new();
    public Dictionary<int, string> Notes { get; set; } = new();
    public UserSettings Settings { get; set; } = new();
}
