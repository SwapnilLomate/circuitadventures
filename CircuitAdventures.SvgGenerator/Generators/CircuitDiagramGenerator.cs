using CircuitAdventures.SvgGenerator.Components;
using CircuitAdventures.SvgGenerator.Models;

namespace CircuitAdventures.SvgGenerator.Generators;

/// <summary>
/// Generates circuit diagrams for each step of a level
/// </summary>
public class CircuitDiagramGenerator
{
    private const int CanvasWidth = 800;
    private const int CanvasHeight = 600;

    private readonly Level _level;
    private readonly Dictionary<string, ComponentRenderer> _components = new();
    private readonly List<WireRenderer> _wires = new();

    public CircuitDiagramGenerator(Level level)
    {
        _level = level;
        LayoutComponents();
    }

    /// <summary>
    /// Layout components on the canvas
    /// </summary>
    private void LayoutComponents()
    {
        var componentList = _level.Components;
        var centerX = CanvasWidth / 2;
        var centerY = CanvasHeight / 2;

        // Count component types by summing Quantity
        var ledCount = componentList.Where(c => c.Name.Contains("LED", StringComparison.OrdinalIgnoreCase)).Sum(c => c.Quantity);
        var batteryCount = componentList.Where(c => c.Name.Contains("Battery", StringComparison.OrdinalIgnoreCase) || c.Name.Contains("AA", StringComparison.OrdinalIgnoreCase)).Sum(c => c.Quantity);
        var resistorCount = componentList.Where(c => c.Name.Contains("Resistor", StringComparison.OrdinalIgnoreCase)).Sum(c => c.Quantity);
        var switchCount = componentList.Where(c => c.Name.Contains("Switch", StringComparison.OrdinalIgnoreCase) && !c.Name.Contains("Push", StringComparison.OrdinalIgnoreCase) && !c.Name.Contains("Button", StringComparison.OrdinalIgnoreCase)).Sum(c => c.Quantity);
        var buttonCount = componentList.Where(c => c.Name.Contains("Button", StringComparison.OrdinalIgnoreCase) || c.Name.Contains("Push", StringComparison.OrdinalIgnoreCase)).Sum(c => c.Quantity);
        var buzzerCount = componentList.Where(c => c.Name.Contains("Buzzer", StringComparison.OrdinalIgnoreCase)).Sum(c => c.Quantity);

        // Battery placement (left side, vertically centered)
        var batteryX = 150.0;
        var batteryY = centerY;

        if (batteryCount == 1)
        {
            _components["battery1"] = new BatteryRenderer(batteryX, batteryY) { Label = "Battery" };
        }
        else if (batteryCount == 2)
        {
            _components["battery1"] = new BatteryRenderer(batteryX, batteryY - 80) { Label = "Battery 1" };
            _components["battery2"] = new BatteryRenderer(batteryX, batteryY + 80) { Label = "Battery 2" };
        }

        // LED placement (right side)
        var ledX = CanvasWidth - 150;

        if (ledCount == 1)
        {
            _components["led1"] = new LedRenderer(ledX, centerY) { Label = "LED" };
        }
        else if (ledCount == 2)
        {
            _components["led1"] = new LedRenderer(ledX, centerY - 80) { Label = "LED 1" };
            _components["led2"] = new LedRenderer(ledX, centerY + 80) { Label = "LED 2" };
        }
        else if (ledCount == 3)
        {
            _components["led1"] = new LedRenderer(ledX, centerY - 100) { Label = "LED 1" };
            _components["led2"] = new LedRenderer(ledX, centerY) { Label = "LED 2" };
            _components["led3"] = new LedRenderer(ledX, centerY + 100) { Label = "LED 3" };
        }

        // Resistor placement (to the right, between switches/button and LED)
        var resistorX = ledX - 150;
        var resistorY = centerY - 100;

        if (resistorCount >= 1)
        {
            _components["resistor1"] = new ResistorRenderer(resistorX, resistorY) { Label = resistorCount > 1 ? "330Ω" : "Resistor" };
        }
        if (resistorCount >= 2)
        {
            // Check if this is a comparison level (like level 6) or a multi-resistor level (like level 10)
            var hasMultipleSameValueResistors = ledCount >= 3 && resistorCount >= 3;
            _components["resistor2"] = new ResistorRenderer(resistorX, centerY) { Label = hasMultipleSameValueResistors ? "330Ω" : "1KΩ" };
        }
        if (resistorCount >= 3)
        {
            _components["resistor3"] = new ResistorRenderer(resistorX, centerY + 100) { Label = "330Ω" };
        }

        // Switch placement (between battery and resistor, middle area)
        var switchX = centerX - 80;
        var switchY = centerY;

        if (switchCount >= 1)
        {
            _components["switch1"] = new SwitchRenderer(switchX, switchY) { Label = switchCount > 1 ? "Switch 1" : "Switch" };
        }
        if (switchCount >= 2)
        {
            _components["switch2"] = new SwitchRenderer(switchX + 120, switchY) { Label = "Switch 2" };
        }

        // Button placement (similar to switch position)
        if (buttonCount >= 1)
        {
            _components["button1"] = new PushButtonRenderer(centerX, switchY) { Label = "Button" };
        }

        // Buzzer placement (instead of LED if present)
        if (buzzerCount >= 1)
        {
            _components["buzzer1"] = new BuzzerRenderer(ledX, centerY) { Label = "Buzzer" };
            // Remove LED if buzzer is present
            _components.Remove("led1");
        }
    }

    /// <summary>
    /// Generate step diagram SVG
    /// </summary>
    public string GenerateStepDiagram(int stepNumber)
    {
        var svg = new SvgBuilder(CanvasWidth, CanvasHeight);
        svg.StartSvg();

        // Add title
        var instruction = _level.Instructions.FirstOrDefault(i => i.StepNumber == stepNumber);
        if (instruction != null)
        {
            svg.AddText(CanvasWidth / 2, 40, $"Step {stepNumber}: {instruction.Title}", 24, "#1976D2", "middle", "bold");

            // Add description
            WrapText(svg, instruction.Description, CanvasWidth / 2, 560, 600, 14);
        }

        // Determine which components and wires to show based on step
        DetermineActiveComponents(stepNumber);

        // Render wires first (so they appear behind components)
        foreach (var wire in _wires)
        {
            wire.Render(svg);
        }

        // Render components
        foreach (var component in _components.Values)
        {
            component.Render(svg);
        }

        // Add tip if present
        if (instruction?.Tip != null)
        {
            svg.AddRectangle(30, CanvasHeight - 90, CanvasWidth - 60, 60, "#FFF9C4", "#F57F17", 2, 8);
            svg.AddText(50, CanvasHeight - 65, "💡 Tip:", 14, "#F57F17", "start", "bold");
            WrapText(svg, instruction.Tip, 50, CanvasHeight - 45, CanvasWidth - 100, 12, "#333", "start");
        }

        svg.EndSvg();
        return svg.Build();
    }

    /// <summary>
    /// Generate main diagram (complete circuit)
    /// </summary>
    public string GenerateMainDiagram()
    {
        var svg = new SvgBuilder(CanvasWidth, CanvasHeight);
        svg.StartSvg();

        // Title
        svg.AddText(CanvasWidth / 2, 40, _level.Title, 28, "#1976D2", "middle", "bold");
        svg.AddText(CanvasWidth / 2, 70, "Complete Circuit Diagram", 18, "#666", "middle");

        // Show all components and connections
        DetermineActiveComponents(_level.Instructions.Count);

        // Render all wires
        foreach (var wire in _wires)
        {
            wire.Render(svg);
        }

        // Render all components
        foreach (var component in _components.Values)
        {
            component.Render(svg);
        }

        svg.EndSvg();
        return svg.Build();
    }

    /// <summary>
    /// Generate final view (realistic completed circuit)
    /// </summary>
    public string GenerateFinalView()
    {
        var svg = new SvgBuilder(CanvasWidth, CanvasHeight);
        svg.StartSvg();

        // Title
        svg.AddText(CanvasWidth / 2, 40, "Your Completed Circuit!", 28, "#4CAF50", "middle", "bold");

        // Show completed circuit with all LEDs lit
        DetermineActiveComponents(_level.Instructions.Count, allLit: true);

        // Render all wires
        foreach (var wire in _wires)
        {
            wire.Render(svg);
        }

        // Render all components (with LEDs lit, switches on, etc.)
        foreach (var component in _components.Values)
        {
            if (component is LedRenderer led)
            {
                led.IsLit = true;
            }
            else if (component is SwitchRenderer switchComp)
            {
                switchComp.IsOn = true;
            }
            else if (component is BuzzerRenderer buzzer)
            {
                buzzer.IsActive = true;
            }

            component.Render(svg);
        }

        // Success message
        svg.AddRectangle(CanvasWidth / 2 - 200, CanvasHeight - 80, 400, 50, "#C8E6C9", "#388E3C", 2, 8);
        svg.AddText(CanvasWidth / 2, CanvasHeight - 50, "✓ Circuit Complete! Well Done!", 18, "#1B5E20", "middle", "bold");

        svg.EndSvg();
        return svg.Build();
    }

    /// <summary>
    /// Determine which components and connections are active for a given step
    /// </summary>
    private void DetermineActiveComponents(int stepNumber, bool allLit = false)
    {
        // Clear previous wires
        _wires.Clear();

        // Reset highlights
        foreach (var comp in _components.Values)
        {
            comp.IsHighlighted = false;
        }

        // Check if current step is a "disconnect" step - if so, don't build progressive connections
        var currentInstruction = _level.Instructions.FirstOrDefault(i => i.StepNumber == stepNumber);
        var currentDesc = currentInstruction?.Description.ToLower() ?? "";

        if (currentDesc.Contains("disconnect"))
        {
            // For disconnect steps, only show components, no wires
            return;
        }

        // Analyze instructions to build connections progressively
        for (int i = 1; i <= stepNumber && i <= _level.Instructions.Count; i++)
        {
            var instruction = _level.Instructions[i - 1];
            var isCurrentStep = (i == stepNumber);
            var instDesc = instruction.Description.ToLower();

            // Skip steps that disconnect or replace
            if (instDesc.Contains("disconnect") && i < stepNumber)
            {
                // If there was a disconnect before current step, clear wires
                _wires.Clear();
                continue;
            }

            AddConnectionsForStep(instruction, isCurrentStep);
        }
    }

    /// <summary>
    /// Add wire connections based on instruction description
    /// </summary>
    private void AddConnectionsForStep(Instruction instruction, bool highlight)
    {
        var desc = instruction.Description.ToLower();
        var title = instruction.Title.ToLower();

        // Pattern matching for common connection descriptions
        // IMPORTANT: Specific patterns MUST come before general patterns!

        // Handle "second led" patterns first (most specific)
        if (desc.Contains("second led") && desc.Contains("long") && desc.Contains("positive"))
        {
            // Parallel circuit - second LED to battery positive
            if (_components.TryGetValue("battery1", out var battery) && _components.TryGetValue("led2", out var led2))
            {
                var led2Comp = (LedRenderer)led2;
                var wire = new WireRenderer(battery.X, battery.Y - 60, led2Comp.X + 12, led2Comp.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    led2.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("second led") && desc.Contains("short") && desc.Contains("negative"))
        {
            // Second LED short leg to battery negative
            if (_components.TryGetValue("led2", out var led2))
            {
                // If there are 2 batteries (series), connect to battery 2's negative (free negative)
                // Otherwise connect to battery 1's negative
                var battery2 = _components.GetValueOrDefault("battery2");
                var battery1 = _components.GetValueOrDefault("battery1");
                var targetBattery = battery2 ?? battery1;

                if (targetBattery != null)
                {
                    var led2Comp = (LedRenderer)led2;
                    var wire = new WireRenderer(led2Comp.X - 12, led2Comp.Y + 45, targetBattery.X, targetBattery.Y + 55)
                    {
                        IsHighlighted = highlight,
                        Color = "#2196F3"
                    };
                    _wires.Add(wire);

                    if (highlight)
                    {
                        targetBattery.IsHighlighted = true;
                        led2.IsHighlighted = true;
                    }
                }
            }
        }
        // Handle "first led" patterns
        else if (desc.Contains("first led") && desc.Contains("battery") && desc.Contains("positive"))
        {
            // Battery positive to first LED
            if (_components.TryGetValue("battery1", out var battery) && _components.TryGetValue("led1", out var led1))
            {
                var led1Comp = (LedRenderer)led1;
                var wire = new WireRenderer(battery.X, battery.Y - 60, led1Comp.X + 12, led1Comp.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    led1.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("first led") && desc.Contains("short") && desc.Contains("negative"))
        {
            // First LED short leg to battery/component
            if (_components.TryGetValue("led1", out var led1))
            {
                var led1Comp = (LedRenderer)led1;

                if (desc.Contains("battery"))
                {
                    var battery = _components.GetValueOrDefault("battery1");
                    var battery2 = _components.GetValueOrDefault("battery2");
                    var targetBattery = battery2 ?? battery;

                    if (targetBattery != null)
                    {
                        var wire = new WireRenderer(led1Comp.X - 12, led1Comp.Y + 45, targetBattery.X, targetBattery.Y + 55)
                        {
                            IsHighlighted = highlight,
                            Color = "#2196F3"
                        };
                        _wires.Add(wire);

                        if (highlight)
                        {
                            led1.IsHighlighted = true;
                            targetBattery.IsHighlighted = true;
                        }
                    }
                }
                else if (desc.Contains("second led"))
                {
                    // LED1 short to LED2 long (series circuit)
                    if (_components.TryGetValue("led2", out var led2))
                    {
                        var led2Comp = (LedRenderer)led2;
                        var wire = new WireRenderer(led1Comp.X - 12, led1Comp.Y + 45, led2Comp.X + 12, led2Comp.Y + 55)
                        {
                            IsHighlighted = highlight,
                            Color = "#9C27B0"
                        };
                        _wires.Add(wire);

                        if (highlight)
                        {
                            led1.IsHighlighted = true;
                            led2.IsHighlighted = true;
                        }
                    }
                }
            }
        }
        // General pattern for single LED (comes after specific patterns)
        else if (desc.Contains("battery") && desc.Contains("led") && desc.Contains("positive") && !desc.Contains("first") && !desc.Contains("second"))
        {
            // Battery positive to LED positive
            if (_components.TryGetValue("battery1", out var battery) && _components.TryGetValue("led1", out var led))
            {
                var ledComponent = (LedRenderer)led;
                var wire = new WireRenderer(battery.X, battery.Y - 60, ledComponent.X + 12, ledComponent.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336" // Red for positive
                };
                _wires.Add(wire);
            }
        }
        else if (desc.Contains("battery") && desc.Contains("led") && desc.Contains("negative") && !desc.Contains("first") && !desc.Contains("second"))
        {
            // LED negative to battery negative
            if (_components.TryGetValue("battery1", out var battery) && _components.TryGetValue("led1", out var led))
            {
                var ledComponent = (LedRenderer)led;
                var wire = new WireRenderer(ledComponent.X - 12, ledComponent.Y + 45, battery.X, battery.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#2196F3" // Blue for negative
                };
                _wires.Add(wire);
            }
        }
        else if (desc.Contains("battery") && desc.Contains("resistor"))
        {
            // Battery to resistor
            // Check if this is a replacement scenario (use resistor2 if step mentions "1k" or if step 4+)
            ComponentRenderer? resistor = null;

            if ((desc.Contains("1k") || desc.Contains("reconnect") || instruction.StepNumber >= 4) &&
                _components.ContainsKey("resistor2"))
            {
                _components.TryGetValue("resistor2", out resistor);
            }
            else
            {
                _components.TryGetValue("resistor1", out resistor);
            }

            if (_components.TryGetValue("battery1", out var battery) && resistor != null)
            {
                var wire = new WireRenderer(battery.X, battery.Y - 60, resistor.X - 50, resistor.Y)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336" // Red for positive
                };
                _wires.Add(wire);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    resistor.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("battery") && desc.Contains("switch"))
        {
            // Battery to switch
            if (_components.TryGetValue("battery1", out var battery) &&
                (_components.TryGetValue("switch1", out var switch1) || _components.TryGetValue("button1", out switch1)))
            {
                var wire = new WireRenderer(battery.X, battery.Y - 60, switch1.X - 45, switch1.Y)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    switch1.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("second switch") && desc.Contains("resistor"))
        {
            // Second switch to resistor
            if (_components.TryGetValue("switch2", out var switch2) && _components.TryGetValue("resistor1", out var resistor))
            {
                var wire = new WireRenderer(switch2.X + 45, switch2.Y, resistor.X - 50, resistor.Y)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    switch2.IsHighlighted = true;
                    resistor.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("switch") && (desc.Contains("led") || desc.Contains("resistor")))
        {
            // Switch to LED or resistor (general pattern)
            ComponentRenderer? fromComp = null;
            _components.TryGetValue("switch1", out fromComp);
            fromComp ??= _components.GetValueOrDefault("button1");

            ComponentRenderer? toComp = null;
            _components.TryGetValue("resistor1", out toComp);
            toComp ??= _components.GetValueOrDefault("led1");

            if (fromComp != null && toComp != null)
            {
                double toX = toComp.X;
                double toY = toComp.Y;

                if (toComp is LedRenderer ledComp)
                {
                    toX = ledComp.X + 12;
                    toY = ledComp.Y + 55;
                }
                else if (toComp is ResistorRenderer)
                {
                    toX -= 50;
                }

                var wire = new WireRenderer(fromComp.X + 45, fromComp.Y, toX, toY)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    fromComp.IsHighlighted = true;
                    toComp.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("three") && desc.Contains("parallel") && desc.Contains("resistor"))
        {
            // Special case: 3 LEDs in parallel, each with its own resistor (MUST come before general resistor+led pattern)
            for (int i = 1; i <= 3; i++)
            {
                var ledKey = $"led{i}";
                var resistorKey = $"resistor{i}";

                if (_components.ContainsKey(ledKey) && _components.ContainsKey(resistorKey))
                {
                    var led = _components[ledKey];
                    var resistor = _components[resistorKey];
                    var ledComp = (LedRenderer)led;

                    // Connect resistor to LED
                    var wire = new WireRenderer(resistor.X + 50, resistor.Y, ledComp.X + 12, ledComp.Y + 55)
                    {
                        IsHighlighted = highlight,
                        Color = "#F44336"
                    };
                    _wires.Add(wire);

                    if (highlight && i == 1)
                    {
                        // Only highlight first pair for clarity
                        resistor.IsHighlighted = true;
                        led.IsHighlighted = true;
                    }
                }
            }
        }
        else if (desc.Contains("resistor") && desc.Contains("led"))
        {
            // Resistor to LED (general pattern - comes after specific patterns)
            // Check if this is a replacement scenario (use resistor2 if step mentions "1k" or if step 4+)
            ComponentRenderer? resistor = null;

            if ((desc.Contains("1k") || desc.Contains("reconnect") || instruction.StepNumber >= 4) &&
                _components.ContainsKey("resistor2"))
            {
                _components.TryGetValue("resistor2", out resistor);
            }
            else
            {
                _components.TryGetValue("resistor1", out resistor);
            }

            if (resistor != null && _components.TryGetValue("led1", out var led))
            {
                var ledComponent = (LedRenderer)led;
                var wire = new WireRenderer(resistor.X + 50, resistor.Y, ledComponent.X + 12, ledComponent.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    resistor.IsHighlighted = true;
                    led.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("batteries") && desc.Contains("series"))
        {
            // Connect two batteries in series
            if (_components.TryGetValue("battery1", out var bat1) && _components.TryGetValue("battery2", out var bat2))
            {
                var wire = new WireRenderer(bat1.X, bat1.Y + 55, bat2.X, bat2.Y - 60)
                {
                    IsHighlighted = highlight,
                    Color = "#9C27B0"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    bat1.IsHighlighted = true;
                    bat2.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("buzzer"))
        {
            // Connect to buzzer
            if (_components.TryGetValue("buzzer1", out var buzzer))
            {
                if (desc.Contains("button") && _components.TryGetValue("button1", out var button))
                {
                    var buzzerComp = (BuzzerRenderer)buzzer;
                    var wire = new WireRenderer(button.X + 20, button.Y + 50, buzzerComp.X - 15, buzzerComp.Y + 60)
                    {
                        IsHighlighted = highlight,
                        Color = "#F44336"
                    };
                    _wires.Add(wire);

                    if (highlight)
                    {
                        button.IsHighlighted = true;
                        buzzer.IsHighlighted = true;
                    }
                }
                else if (desc.Contains("negative") && _components.TryGetValue("battery1", out var battery))
                {
                    var buzzerComp = (BuzzerRenderer)buzzer;
                    var wire = new WireRenderer(buzzerComp.X + 15, buzzerComp.Y + 60, battery.X, battery.Y + 55)
                    {
                        IsHighlighted = highlight,
                        Color = "#2196F3"
                    };
                    _wires.Add(wire);

                    if (highlight)
                    {
                        buzzer.IsHighlighted = true;
                        battery.IsHighlighted = true;
                    }
                }
            }
        }
        else if ((desc.Contains("first switch") && desc.Contains("second switch")) ||
                 (desc.Contains("switch") && desc.Contains("switch") && desc.Contains("series")))
        {
            // Connect switch1 to switch2
            if (_components.TryGetValue("switch1", out var switch1) && _components.TryGetValue("switch2", out var switch2))
            {
                var wire = new WireRenderer(switch1.X + 45, switch1.Y, switch2.X - 45, switch2.Y)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    switch1.IsHighlighted = true;
                    switch2.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("battery") && desc.Contains("button"))
        {
            // Battery to button
            if (_components.TryGetValue("battery1", out var battery) && _components.TryGetValue("button1", out var button))
            {
                var wire = new WireRenderer(battery.X, battery.Y - 60, button.X - 20, button.Y + 50)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    button.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("button") && desc.Contains("resistor"))
        {
            // Button to resistor
            if (_components.TryGetValue("button1", out var button) && _components.TryGetValue("resistor1", out var resistor))
            {
                var wire = new WireRenderer(button.X + 20, button.Y + 50, resistor.X - 50, resistor.Y)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    button.IsHighlighted = true;
                    resistor.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("led") && desc.Contains("led") && desc.Contains("short") && desc.Contains("long"))
        {
            // LED to LED connection (series circuit)
            if (_components.TryGetValue("led1", out var led1) && _components.TryGetValue("led2", out var led2))
            {
                var led1Comp = (LedRenderer)led1;
                var led2Comp = (LedRenderer)led2;

                // Connect LED1 short leg (negative) to LED2 long leg (positive)
                var wire = new WireRenderer(led1Comp.X - 12, led1Comp.Y + 45, led2Comp.X + 12, led2Comp.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#9C27B0" // Purple for LED-to-LED connection
                };
                _wires.Add(wire);

                if (highlight)
                {
                    led1.IsHighlighted = true;
                    led2.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("second led") && desc.Contains("long") && desc.Contains("positive"))
        {
            // Parallel circuit - second LED to battery positive (MUST come before general pattern)
            if (_components.TryGetValue("battery1", out var battery) && _components.TryGetValue("led2", out var led2))
            {
                var led2Comp = (LedRenderer)led2;
                var wire = new WireRenderer(battery.X, battery.Y - 60, led2Comp.X + 12, led2Comp.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    led2.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("second led") && desc.Contains("short") && desc.Contains("negative"))
        {
            // Parallel circuit - second LED short leg to battery negative (MUST come before general pattern)
            if (_components.TryGetValue("battery1", out var battery) && _components.TryGetValue("led2", out var led2))
            {
                var led2Comp = (LedRenderer)led2;
                var wire = new WireRenderer(led2Comp.X - 12, led2Comp.Y + 45, battery.X, battery.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#2196F3"
                };
                _wires.Add(wire);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    led2.IsHighlighted = true;
                }
            }
        }
        else if ((desc.Contains("first led") || desc.Contains("second led")) && desc.Contains("battery"))
        {
            // Battery to specific LED connections (general pattern - comes after specific second led patterns)
            var ledNum = desc.Contains("first") ? "1" : (desc.Contains("second") ? "2" : "1");

            if (_components.TryGetValue($"led{ledNum}", out var led))
            {
                if (desc.Contains("positive") && desc.Contains("long"))
                {
                    // Battery positive to LED long leg
                    var battery = _components.GetValueOrDefault("battery1");
                    if (battery != null)
                    {
                        var ledComp = (LedRenderer)led;
                        var wire = new WireRenderer(battery.X, battery.Y - 60, ledComp.X + 12, ledComp.Y + 55)
                        {
                            IsHighlighted = highlight,
                            Color = "#F44336"
                        };
                        _wires.Add(wire);

                        if (highlight)
                        {
                            battery.IsHighlighted = true;
                            led.IsHighlighted = true;
                        }
                    }
                }
                else if (desc.Contains("negative") && desc.Contains("short"))
                {
                    // LED short leg to battery negative
                    var battery = _components.GetValueOrDefault("battery1");
                    var battery2 = _components.GetValueOrDefault("battery2");
                    var targetBattery = battery2 ?? battery; // Use battery2 if it exists (series), otherwise battery1

                    if (targetBattery != null)
                    {
                        var ledComp = (LedRenderer)led;
                        var wire = new WireRenderer(ledComp.X - 12, ledComp.Y + 45, targetBattery.X, targetBattery.Y + 55)
                        {
                            IsHighlighted = highlight,
                            Color = "#2196F3"
                        };
                        _wires.Add(wire);

                        if (highlight)
                        {
                            targetBattery.IsHighlighted = true;
                            led.IsHighlighted = true;
                        }
                    }
                }
            }
        }
        else if (desc.Contains("reconnect") && desc.Contains("battery"))
        {
            // Reconnecting battery - rebuild full circuit with appropriate resistor
            // Use resistor2 if it exists and we're past the replacement step
            ComponentRenderer? resistor = null;

            if (instruction.StepNumber >= 4 && _components.ContainsKey("resistor2"))
            {
                _components.TryGetValue("resistor2", out resistor);
            }
            else
            {
                _components.TryGetValue("resistor1", out resistor);
            }

            if (_components.TryGetValue("battery1", out var battery) && resistor != null && _components.TryGetValue("led1", out var led))
            {
                var ledComp = (LedRenderer)led;

                // Battery to resistor
                var wire1 = new WireRenderer(battery.X, battery.Y - 60, resistor.X - 50, resistor.Y)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire1);

                // Resistor to LED
                var wire2 = new WireRenderer(resistor.X + 50, resistor.Y, ledComp.X + 12, ledComp.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#F44336"
                };
                _wires.Add(wire2);

                // LED to battery negative
                var wire3 = new WireRenderer(ledComp.X - 12, ledComp.Y + 45, battery.X, battery.Y + 55)
                {
                    IsHighlighted = highlight,
                    Color = "#2196F3"
                };
                _wires.Add(wire3);

                if (highlight)
                {
                    battery.IsHighlighted = true;
                    resistor.IsHighlighted = true;
                    led.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("switch") && desc.Contains("battery pack") && desc.Contains("led array"))
        {
            // Switch between battery and LED array (flashlight level 10 step 5)
            if (_components.TryGetValue("switch1", out var switch1))
            {
                // Connect battery to switch
                if (_components.TryGetValue("battery1", out var battery))
                {
                    var wire1 = new WireRenderer(battery.X, battery.Y - 60, switch1.X - 45, switch1.Y)
                    {
                        IsHighlighted = highlight,
                        Color = "#F44336"
                    };
                    _wires.Add(wire1);
                }

                // Connect switch to LED array (via resistors)
                if (_components.TryGetValue("resistor1", out var resistor))
                {
                    var wire2 = new WireRenderer(switch1.X + 45, switch1.Y, resistor.X - 50, resistor.Y)
                    {
                        IsHighlighted = highlight,
                        Color = "#F44336"
                    };
                    _wires.Add(wire2);
                }

                if (highlight)
                {
                    switch1.IsHighlighted = true;
                }
            }
        }
        else if (desc.Contains("flip") || desc.Contains("test") || desc.Contains("press") ||
                 desc.Contains("lights") || desc.Contains("light up") || desc.Contains("should") ||
                 desc.Contains("arrange") || desc.Contains("mount") || desc.Contains("tape") ||
                 desc.Contains("secure") || desc.Contains("cut") || desc.Contains("line"))
        {
            // Testing or assembly step - don't add new connections, just show completed circuit
            // Mark circuit as complete by showing all components active if it's the last step
        }

        // If this is step 1 and it's about identifying components, highlight them
        if (instruction.StepNumber == 1 && (desc.Contains("look") || desc.Contains("identify") || desc.Contains("examine")))
        {
            if (desc.Contains("led") && _components.TryGetValue("led1", out var led))
            {
                led.IsHighlighted = highlight;
            }
            if (desc.Contains("battery") && _components.TryGetValue("battery1", out var battery))
            {
                battery.IsHighlighted = highlight;
            }
            if (desc.Contains("resistor") && _components.TryGetValue("resistor1", out var resistor))
            {
                resistor.IsHighlighted = highlight;
            }
            if (desc.Contains("button") && _components.TryGetValue("button1", out var button))
            {
                button.IsHighlighted = highlight;
            }
            if (desc.Contains("buzzer") && _components.TryGetValue("buzzer1", out var buzzer))
            {
                buzzer.IsHighlighted = highlight;
            }
        }
    }

    /// <summary>
    /// Helper to wrap text to multiple lines
    /// </summary>
    private void WrapText(SvgBuilder svg, string text, double x, double y, double maxWidth, int fontSize, string fill = "#666", string anchor = "middle")
    {
        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = "";
        var charWidth = fontSize * 0.5; // Approximate character width

        foreach (var word in words)
        {
            var testLine = currentLine.Length > 0 ? currentLine + " " + word : word;
            if (testLine.Length * charWidth > maxWidth && currentLine.Length > 0)
            {
                lines.Add(currentLine);
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }

        if (currentLine.Length > 0)
        {
            lines.Add(currentLine);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            svg.AddText(x, y + (i * (fontSize + 4)), lines[i], fontSize, fill, anchor);
        }
    }
}
