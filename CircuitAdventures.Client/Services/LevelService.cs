using CircuitAdventures.Client.Models;

namespace CircuitAdventures.Client.Services;

/// <summary>
/// Service for managing level data
/// </summary>
public class LevelService
{
    private List<Level> _levels = new();

    public LevelService()
    {
        // Levels will be loaded from LevelsData.cs
        LoadLevels();
    }

    /// <summary>
    /// Get all levels
    /// </summary>
    public List<Level> GetAllLevels()
    {
        return _levels;
    }

    /// <summary>
    /// Get a specific level by ID
    /// </summary>
    public Level? GetLevelById(int id)
    {
        return _levels.FirstOrDefault(l => l.Id == id);
    }

    /// <summary>
    /// Get levels by category
    /// </summary>
    public List<Level> GetLevelsByCategory(string category)
    {
        return _levels.Where(l => l.Category == category).ToList();
    }

    /// <summary>
    /// Get all unique categories
    /// </summary>
    public List<string> GetAllCategories()
    {
        return _levels.Select(l => l.Category).Distinct().ToList();
    }

    /// <summary>
    /// Load levels data (to be populated from Data/LevelsData.cs)
    /// </summary>
    private void LoadLevels()
    {
        // Load defined levels from data
        _levels = Data.LevelsData.GetLevels();

        // Generate placeholder levels for remaining levels (up to 100)
        var existingCount = _levels.Count;

        for (int i = existingCount + 1; i <= 100; i++)
        {
            var category = i switch
            {
                <= 20 => "Beginner Zone",
                <= 35 => "Energy Explorer",
                <= 50 => "Motion Maker",
                <= 65 => "Light Master",
                <= 80 => "Circuit Wizard",
                _ => "Innovation Lab"
            };

            _levels.Add(new Level
            {
                Id = i,
                Title = $"Level {i} Project",
                Category = category,
                Difficulty = (i / 20) + 1,
                EstimatedTime = 10 + (i / 10) * 5,
                Components = new(),
                Instructions = new(),
                LearningObjectives = new List<string> { "Coming soon..." },
                SafetyNotes = new List<string> { "Ask an adult for help" }
            });
        }
    }
}
