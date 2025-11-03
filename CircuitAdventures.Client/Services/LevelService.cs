using CircuitAdventures.Client.Models;
using System.Net.Http.Json;

namespace CircuitAdventures.Client.Services;

/// <summary>
/// Service for managing level data
/// </summary>
public class LevelService
{
    private readonly HttpClient _http;
    private List<Level> _levels = new();
    private bool _isLoaded = false;

    public LevelService(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Initialize and load all levels from JSON files
    /// </summary>
    public async Task InitializeAsync()
    {
        if (_isLoaded) return;

        await LoadLevelsFromJson();
        _isLoaded = true;
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
    /// Load levels from JSON files
    /// </summary>
    private async Task LoadLevelsFromJson()
    {
        _levels = new List<Level>();

        // Load from 10 JSON files
        for (int i = 1; i <= 10; i++)
        {
            try
            {
                var fileNumber = i.ToString("00");
                var url = $"data/levels/levels-{fileNumber}.json";
                var levels = await _http.GetFromJsonAsync<List<Level>>(url);

                if (levels != null && levels.Count > 0)
                {
                    _levels.AddRange(levels);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading levels file {i}: {ex.Message}");
            }
        }

        // Generate placeholder levels for any missing levels (up to 100)
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

        // Sort by ID to ensure correct order
        _levels = _levels.OrderBy(l => l.Id).ToList();
    }
}
