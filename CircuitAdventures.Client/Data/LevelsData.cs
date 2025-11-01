using CircuitAdventures.Client.Models;

namespace CircuitAdventures.Client.Data;

/// <summary>
/// Static data class containing all 100 levels
/// </summary>
public static class LevelsData
{
    public static List<Level> GetLevels()
    {
        return new List<Level>
        {
            // Level 1: First Light
            new()
            {
                Id = 1,
                Title = "First Light",
                Category = "Beginner Zone",
                Difficulty = 1,
                EstimatedTime = 5,
                Components = new List<ComponentItem>
                {
                    new() { Name = "LED", Quantity = 1, ImageUrl = "/images/components/led.svg", Description = "A light emitting diode" },
                    new() { Name = "AA Battery", Quantity = 1, ImageUrl = "/images/components/battery.svg", Description = "1.5V power source" },
                    new() { Name = "Jumper Wires", Quantity = 2, ImageUrl = "/images/components/wire.svg", Description = "Connecting wires" }
                },
                AdditionalMaterials = new List<string>(),
                LearningObjectives = new List<string>
                {
                    "Understand what a circuit is",
                    "Learn about positive and negative terminals",
                    "Identify LED polarity (long leg = positive, short leg = negative)"
                },
                SafetyNotes = new List<string>
                {
                    "Never connect battery terminals directly without a component",
                    "Batteries can get hot if short-circuited",
                    "Always ask an adult for help if unsure"
                },
                Instructions = new List<InstructionStep>
                {
                    new() { StepNumber = 1, Title = "Identify LED Legs", Description = "Look at your LED. The longer leg is positive (+), and the shorter leg is negative (-).", DiagramUrl = "/images/levels/level-001/step-1.svg", Tip = "Hold the LED up to the light to see the legs clearly!" },
                    new() { StepNumber = 2, Title = "Connect Positive", Description = "Connect one wire from the battery positive (+) terminal to the LED long leg.", DiagramUrl = "/images/levels/level-001/step-2.svg" },
                    new() { StepNumber = 3, Title = "Connect Negative", Description = "Connect another wire from the LED short leg to the battery negative (-) terminal.", DiagramUrl = "/images/levels/level-001/step-3.svg" },
                    new() { StepNumber = 4, Title = "See the Light!", Description = "Your LED should light up! If it doesn't, check your connections.", DiagramUrl = "/images/levels/level-001/step-4.svg", Tip = "If the LED doesn't light up, try reversing the connections!" }
                },
                Diagrams = new List<string>
                {
                    "/images/levels/level-001/main-diagram.svg",
                    "/images/levels/level-001/final-view.svg"
                },
                FunFact = "LEDs use 75% less energy than regular light bulbs and can last for 25 years!",
                Quiz = new QuizQuestion
                {
                    Question = "Which leg of an LED is positive?",
                    Options = new List<string> { "The shorter leg", "The longer leg", "Both legs are the same", "It doesn't matter" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The longer leg of an LED is always the positive terminal. This is how we know which way to connect it in a circuit!"
                }
            },

            // Level 2: Switch It Up
            new()
            {
                Id = 2,
                Title = "Switch It Up",
                Category = "Beginner Zone",
                Difficulty = 1,
                EstimatedTime = 8,
                Components = new List<ComponentItem>
                {
                    new() { Name = "LED", Quantity = 1, ImageUrl = "/images/components/led.svg", Description = "A light emitting diode" },
                    new() { Name = "AA Battery", Quantity = 1, ImageUrl = "/images/components/battery.svg", Description = "1.5V power source" },
                    new() { Name = "Switch", Quantity = 1, ImageUrl = "/images/components/switch.svg", Description = "On/Off control" },
                    new() { Name = "Jumper Wires", Quantity = 3, ImageUrl = "/images/components/wire.svg", Description = "Connecting wires" }
                },
                AdditionalMaterials = new List<string>(),
                LearningObjectives = new List<string>
                {
                    "Understand how switches control circuits",
                    "Learn about open and closed circuits",
                    "Practice building a complete circuit with control"
                },
                SafetyNotes = new List<string>
                {
                    "Make sure switch is off when connecting wires",
                    "Check all connections before turning switch on"
                },
                Instructions = new List<InstructionStep>
                {
                    new() { StepNumber = 1, Title = "Connect Battery to Switch", Description = "Connect a wire from the battery positive (+) to one terminal of the switch.", DiagramUrl = "/images/levels/level-002/step-1.svg" },
                    new() { StepNumber = 2, Title = "Connect Switch to LED", Description = "Connect a wire from the other switch terminal to the LED long leg (positive).", DiagramUrl = "/images/levels/level-002/step-2.svg" },
                    new() { StepNumber = 3, Title = "Complete the Circuit", Description = "Connect a wire from the LED short leg (negative) to the battery negative (-).", DiagramUrl = "/images/levels/level-002/step-3.svg" },
                    new() { StepNumber = 4, Title = "Test Your Switch", Description = "Flip the switch on and off to control the LED!", DiagramUrl = "/images/levels/level-002/step-4.svg", Tip = "Notice how the LED only lights up when the switch is on!" }
                },
                Diagrams = new List<string>
                {
                    "/images/levels/level-002/main-diagram.svg",
                    "/images/levels/level-002/final-view.svg"
                },
                FunFact = "Light switches in your home work the same way - they open and close electrical circuits!",
                Quiz = new QuizQuestion
                {
                    Question = "What does a switch do in a circuit?",
                    Options = new List<string> { "Makes lights brighter", "Opens and closes the circuit", "Changes battery power", "Makes wires longer" },
                    CorrectAnswerIndex = 1,
                    Explanation = "A switch opens and closes the circuit. When it's closed (on), electricity can flow. When it's open (off), the flow stops!"
                }
            },

            // Placeholder for remaining levels (3-100)
            // These will be added later with full content
        };
    }
}
