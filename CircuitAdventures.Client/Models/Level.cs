namespace CircuitAdventures.Client.Models;

/// <summary>
/// Represents a single electronics project level
/// </summary>
public class Level
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Difficulty { get; set; } // 1-5
    public int EstimatedTime { get; set; } // minutes
    public List<ComponentItem> Components { get; set; } = new();
    public List<string> AdditionalMaterials { get; set; } = new();
    public List<string> LearningObjectives { get; set; } = new();
    public List<string> SafetyNotes { get; set; } = new();
    public List<InstructionStep> Instructions { get; set; } = new();
    public List<string> Diagrams { get; set; } = new(); // paths to SVG files
    public string? FunFact { get; set; }
    public Challenge? ChallengeMode { get; set; }
    public int? UnlockRequirement { get; set; } // previous level ID
    public QuizQuestion? Quiz { get; set; }
}
