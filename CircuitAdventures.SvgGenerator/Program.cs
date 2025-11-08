using System.Text.Json;
using CircuitAdventures.SvgGenerator.Generators;
using CircuitAdventures.SvgGenerator.Models;

Console.WriteLine("Circuit Adventures SVG Generator");
Console.WriteLine("=================================\n");

// Get paths
var rootPath = args.Length > 0 ? args[0] : @"D:\Repos\circuitadventures";
var dataPath = Path.Combine(rootPath, "CircuitAdventures.Client", "wwwroot", "data", "levels");
var outputPath = Path.Combine(rootPath, "CircuitAdventures.Client", "wwwroot", "images", "levels");

Console.WriteLine($"Data path: {dataPath}");
Console.WriteLine($"Output path: {outputPath}\n");

// Check if paths exist
if (!Directory.Exists(dataPath))
{
    Console.WriteLine($"Error: Data path not found: {dataPath}");
    return;
}

if (!Directory.Exists(outputPath))
{
    Console.WriteLine($"Error: Output path not found: {outputPath}");
    return;
}

// Find all level JSON files (process only first file for levels 1-10)
var jsonFiles = Directory.GetFiles(dataPath, "levels-*.json").OrderBy(f => f).Take(1).ToArray();

Console.WriteLine($"Found {jsonFiles.Length} JSON file(s) to process\n");

var totalLevels = 0;
var totalDiagrams = 0;

foreach (var jsonFile in jsonFiles)
{
    Console.WriteLine($"Processing: {Path.GetFileName(jsonFile)}");

    try
    {
        // Read and parse JSON
        var json = File.ReadAllText(jsonFile);
        var levels = JsonSerializer.Deserialize<List<Level>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (levels == null)
        {
            Console.WriteLine($"  Error: Could not parse JSON file");
            continue;
        }

        Console.WriteLine($"  Found {levels.Count} levels");

        foreach (var level in levels)
        {
            totalLevels++;
            var levelDir = Path.Combine(outputPath, $"level-{level.Id:D3}");

            if (!Directory.Exists(levelDir))
            {
                Directory.CreateDirectory(levelDir);
            }

            Console.WriteLine($"  Level {level.Id}: {level.Title}");

            var generator = new CircuitDiagramGenerator(level);

            // Generate step diagrams
            foreach (var instruction in level.Instructions)
            {
                var stepFile = Path.Combine(levelDir, $"step-{instruction.StepNumber}.svg");
                var stepSvg = generator.GenerateStepDiagram(instruction.StepNumber);

                File.WriteAllText(stepFile, stepSvg);
                totalDiagrams++;

                Console.WriteLine($"    ✓ Generated step-{instruction.StepNumber}.svg");
            }

            // Generate main diagram
            var mainFile = Path.Combine(levelDir, "main-diagram.svg");
            var mainSvg = generator.GenerateMainDiagram();
            File.WriteAllText(mainFile, mainSvg);
            totalDiagrams++;
            Console.WriteLine($"    ✓ Generated main-diagram.svg");

            // Generate final view
            var finalFile = Path.Combine(levelDir, "final-view.svg");
            var finalSvg = generator.GenerateFinalView();
            File.WriteAllText(finalFile, finalSvg);
            totalDiagrams++;
            Console.WriteLine($"    ✓ Generated final-view.svg");

            Console.WriteLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Error processing file: {ex.Message}");
    }
}

Console.WriteLine("\n=================================");
Console.WriteLine($"Generation Complete!");
Console.WriteLine($"Processed {totalLevels} levels");
Console.WriteLine($"Generated {totalDiagrams} SVG diagrams");
Console.WriteLine("=================================");
