namespace CircuitAdventures.SvgGenerator.Models;

public class Level
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public int EstimatedTime { get; set; }
    public string FunFact { get; set; } = string.Empty;
    public List<string> LearningObjectives { get; set; } = new();
    public List<string> SafetyNotes { get; set; } = new();
    public List<Component> Components { get; set; } = new();
    public List<Instruction> Instructions { get; set; } = new();
    public List<string> Diagrams { get; set; } = new();
    public List<string> AdditionalMaterials { get; set; } = new();
    public Quiz? Quiz { get; set; }
}

public class Component
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

public class Instruction
{
    public int StepNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Tip { get; set; }
    public string DiagramUrl { get; set; } = string.Empty;
}

public class Quiz
{
    public string Question { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int CorrectAnswerIndex { get; set; }
    public string Explanation { get; set; } = string.Empty;
}
