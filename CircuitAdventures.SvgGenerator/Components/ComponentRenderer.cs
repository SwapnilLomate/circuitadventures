namespace CircuitAdventures.SvgGenerator.Components;

/// <summary>
/// Base class for rendering circuit components
/// </summary>
public abstract class ComponentRenderer
{
    public double X { get; set; }
    public double Y { get; set; }
    public bool IsHighlighted { get; set; }
    public string Label { get; set; } = string.Empty;

    protected ComponentRenderer(double x, double y)
    {
        X = x;
        Y = y;
    }

    public abstract void Render(SvgBuilder svg);
}

/// <summary>
/// Renders an LED component with kid-friendly design
/// </summary>
public class LedRenderer : ComponentRenderer
{
    public bool IsLit { get; set; }
    public string Color { get; set; } = "#FFC107"; // Yellow by default

    public LedRenderer(double x, double y) : base(x, y) { }

    public override void Render(SvgBuilder svg)
    {
        // LED dome/bulb shape using rounded top
        var bulbColor = IsLit ? Color : "#E8E8E8";
        var edgeColor = IsLit ? "#FFD54F" : "#BDBDBD";

        // Main LED body (rounded rectangle for dome shape)
        svg.AddRaw($"  <ellipse cx=\"{X}\" cy=\"{Y - 5}\" rx=\"22\" ry=\"28\" fill=\"{bulbColor}\" stroke=\"{edgeColor}\" stroke-width=\"2\" opacity=\"0.9\"/>");

        // LED lens/dome top (more transparent overlay for 3D effect)
        svg.AddRaw($"  <ellipse cx=\"{X}\" cy=\"{Y - 8}\" rx=\"20\" ry=\"25\" fill=\"url(#ledGradient)\" stroke=\"none\"/>");

        // Add gradient definition for LED if not exists (add to defs)
        if (IsLit)
        {
            // Inner glow when lit
            svg.AddCircle(X, Y, 15, Color, null, 0);
            svg.AddRaw($"  <circle cx=\"{X}\" cy=\"{Y}\" r=\"15\" fill=\"{Color}\" opacity=\"0.6\"/>");
            svg.AddRaw($"  <ellipse cx=\"{X - 5}\" cy=\"{Y - 10}\" rx=\"8\" ry=\"12\" fill=\"#FFFFFF\" opacity=\"0.4\"/>");

            // Outer glow rings
            svg.AddRaw($"  <circle cx=\"{X}\" cy=\"{Y}\" r=\"32\" fill=\"{Color}\" opacity=\"0.2\"/>");
            svg.AddRaw($"  <circle cx=\"{X}\" cy=\"{Y}\" r=\"38\" fill=\"{Color}\" opacity=\"0.1\"/>");
        }
        else
        {
            // Shine/reflection spot when off
            svg.AddRaw($"  <ellipse cx=\"{X - 6}\" cy=\"{Y - 12}\" rx=\"6\" ry=\"10\" fill=\"#FFFFFF\" opacity=\"0.3\"/>");
        }

        // LED rim/base (flat bottom part)
        svg.AddRaw($"  <rect x=\"{X - 18}\" y=\"{Y + 20}\" width=\"36\" height=\"8\" fill=\"#757575\" rx=\"2\"/>");
        svg.AddRaw($"  <rect x=\"{X - 16}\" y=\"{Y + 22}\" width=\"32\" height=\"4\" fill=\"#9E9E9E\" rx=\"1\"/>");

        // Long leg (positive) - right side, thicker
        svg.AddLine(X + 12, Y + 28, X + 12, Y + 55, "#888", 4, "round");

        // Short leg (negative) - left side, thicker
        svg.AddLine(X - 12, Y + 28, X - 12, Y + 45, "#888", 4, "round");

        // Terminal dots (connection points)
        svg.AddCircle(X + 12, Y + 55, 5, "#FF5252", "#C62828", 2);
        svg.AddCircle(X - 12, Y + 45, 5, "#2196F3", "#1565C0", 2);

        // Labels
        if (!string.IsNullOrEmpty(Label))
        {
            svg.AddText(X, Y - 50, Label, 14, "#333", "middle", "bold");
        }

        // + and - indicators with better visibility
        svg.AddText(X + 12, Y + 70, "+", 13, "#FF5252", "middle", "bold");
        svg.AddText(X - 12, Y + 60, "-", 13, "#2196F3", "middle", "bold");
    }
}

/// <summary>
/// Renders a battery component
/// </summary>
public class BatteryRenderer : ComponentRenderer
{
    public BatteryRenderer(double x, double y) : base(x, y) { }

    public override void Render(SvgBuilder svg)
    {
        // Battery body
        svg.AddRectangle(X - 30, Y - 50, 60, 100, "#4CAF50", "#2E7D32", 2, 8);

        // Positive terminal (top bump)
        svg.AddRectangle(X - 10, Y - 60, 20, 10, "#FF5252", "#C62828", 2, 2);

        // Negative terminal indicator (bottom)
        svg.AddRectangle(X - 15, Y + 50, 30, 5, "#2196F3", "#1565C0", 2);

        // + and - symbols
        svg.AddText(X, Y - 20, "+", 24, "#fff", "middle", "bold");
        svg.AddText(X, Y + 30, "-", 24, "#fff", "middle", "bold");

        // Terminal connection points
        svg.AddCircle(X, Y - 60, 5, "#FF5252");
        svg.AddCircle(X, Y + 55, 5, "#2196F3");

        // Label
        if (!string.IsNullOrEmpty(Label))
        {
            svg.AddText(X, Y - 75, Label, 14, "#333", "middle", "bold");
        }
    }
}

/// <summary>
/// Renders a resistor component
/// </summary>
public class ResistorRenderer : ComponentRenderer
{
    public string[] ColorBands { get; set; } = { "#FF9800", "#FF9800", "#8B4513" }; // 330 ohm default

    public ResistorRenderer(double x, double y) : base(x, y) { }

    public override void Render(SvgBuilder svg)
    {
        // Resistor body
        svg.AddRectangle(X - 40, Y - 10, 80, 20, "#F5DEB3", "#8B4513", 2, 3);

        // Color bands
        for (int i = 0; i < ColorBands.Length && i < 3; i++)
        {
            svg.AddRectangle(X - 25 + (i * 20), Y - 10, 8, 20, ColorBands[i]);
        }

        // Wire leads
        svg.AddLine(X - 50, Y, X - 40, Y, "#666", 3, "round");
        svg.AddLine(X + 40, Y, X + 50, Y, "#666", 3, "round");

        // Connection points
        svg.AddCircle(X - 50, Y, 4, "#666");
        svg.AddCircle(X + 50, Y, 4, "#666");

        // Label
        if (!string.IsNullOrEmpty(Label))
        {
            svg.AddText(X, Y - 25, Label, 14, "#333", "middle", "bold");
        }
    }
}

/// <summary>
/// Renders a wire/connection
/// </summary>
public class WireRenderer : ComponentRenderer
{
    public double X2 { get; set; }
    public double Y2 { get; set; }
    public string Color { get; set; } = "#E91E63"; // Pink wire

    public WireRenderer(double x1, double y1, double x2, double y2) : base(x1, y1)
    {
        X2 = x2;
        Y2 = y2;
    }

    public override void Render(SvgBuilder svg)
    {
        // Draw wire with smooth curve
        var midX = (X + X2) / 2;
        var midY = (Y + Y2) / 2;

        // Calculate control points for smooth bezier curve
        var dx = X2 - X;
        var dy = Y2 - Y;
        var dist = Math.Sqrt(dx * dx + dy * dy);

        if (dist < 50)
        {
            // Short connection - straight line
            svg.AddLine(X, Y, X2, Y2, Color, 4, "round");
        }
        else
        {
            // Longer connection - curved line
            var cx1 = X + dx * 0.3;
            var cy1 = Y;
            var cx2 = X2 - dx * 0.3;
            var cy2 = Y2;

            svg.AddPath($"M {X} {Y} C {cx1} {cy1}, {cx2} {cy2}, {X2} {Y2}", null, Color, 4);
        }

        // Connection dots
        svg.AddCircle(X, Y, 5, Color, "#C2185B", 2);
        svg.AddCircle(X2, Y2, 5, Color, "#C2185B", 2);
    }
}

/// <summary>
/// Renders a switch component
/// </summary>
public class SwitchRenderer : ComponentRenderer
{
    public bool IsOn { get; set; }

    public SwitchRenderer(double x, double y) : base(x, y) { }

    public override void Render(SvgBuilder svg)
    {
        // Switch body
        svg.AddRectangle(X - 35, Y - 20, 70, 40, "#607D8B", "#37474F", 2, 5);

        // Terminal contacts
        svg.AddCircle(X - 25, Y, 6, "#FFD54F", "#F57F17", 2);
        svg.AddCircle(X + 25, Y, 6, "#FFD54F", "#F57F17", 2);

        // Switch lever
        var leverEndX = IsOn ? X + 15 : X - 15;
        var leverEndY = IsOn ? Y - 15 : Y - 15;

        svg.AddLine(X - 5, Y, leverEndX, leverEndY, "#FF5722", 5, "round");
        svg.AddCircle(leverEndX, leverEndY, 7, "#FF5722", "#BF360C", 2);

        // Connection leads
        svg.AddLine(X - 45, Y, X - 35, Y, "#666", 3, "round");
        svg.AddLine(X + 35, Y, X + 45, Y, "#666", 3, "round");

        // Connection points
        svg.AddCircle(X - 45, Y, 4, "#666");
        svg.AddCircle(X + 45, Y, 4, "#666");

        // Label
        if (!string.IsNullOrEmpty(Label))
        {
            svg.AddText(X, Y - 35, Label, 14, "#333", "middle", "bold");
        }

        // State indicator
        var stateText = IsOn ? "ON" : "OFF";
        var stateColor = IsOn ? "#4CAF50" : "#F44336";
        svg.AddText(X, Y + 40, stateText, 12, stateColor, "middle", "bold");
    }
}

/// <summary>
/// Renders a push button component
/// </summary>
public class PushButtonRenderer : ComponentRenderer
{
    public bool IsPressed { get; set; }

    public PushButtonRenderer(double x, double y) : base(x, y) { }

    public override void Render(SvgBuilder svg)
    {
        // Button body
        svg.AddCircle(X, Y, 30, "#FF5722", "#BF360C", 3);

        // Button cap (moves down when pressed)
        var capY = IsPressed ? Y + 5 : Y - 5;
        svg.AddCircle(X, capY, 20, "#FF7043", "#D84315", 2);

        // Shine effect
        svg.AddCircle(X - 8, capY - 8, 6, "#FFCCBC", null, 0);

        // Connection terminals (4 pins)
        svg.AddCircle(X - 20, Y + 35, 5, "#FFD54F", "#F57F17", 2);
        svg.AddCircle(X + 20, Y + 35, 5, "#FFD54F", "#F57F17", 2);

        // Wire leads
        svg.AddLine(X - 20, Y + 40, X - 20, Y + 50, "#666", 3, "round");
        svg.AddLine(X + 20, Y + 40, X + 20, Y + 50, "#666", 3, "round");

        // Connection points
        svg.AddCircle(X - 20, Y + 50, 4, "#666");
        svg.AddCircle(X + 20, Y + 50, 4, "#666");

        // Label
        if (!string.IsNullOrEmpty(Label))
        {
            svg.AddText(X, Y - 45, Label, 14, "#333", "middle", "bold");
        }

        // State text
        if (IsPressed)
        {
            svg.AddText(X, Y + 70, "PRESSED", 11, "#4CAF50", "middle", "bold");
        }
    }
}

/// <summary>
/// Renders a buzzer component
/// </summary>
public class BuzzerRenderer : ComponentRenderer
{
    public bool IsActive { get; set; }

    public BuzzerRenderer(double x, double y) : base(x, y) { }

    public override void Render(SvgBuilder svg)
    {
        // Buzzer body (cylinder)
        svg.AddCircle(X, Y, 35, "#212121", "#000", 2);

        // Top surface
        svg.AddCircle(X, Y - 5, 30, "#424242", "#212121", 2);

        // Center hole
        svg.AddCircle(X, Y - 5, 15, "#757575", "#424242", 1);

        // Sound waves when active
        if (IsActive)
        {
            for (int i = 1; i <= 3; i++)
            {
                var radius = 40 + (i * 12);
                var opacity = 0.4 - (i * 0.1);
                svg.AddRaw($"  <circle cx=\"{X}\" cy=\"{Y - 5}\" r=\"{radius}\" fill=\"none\" stroke=\"#FFC107\" stroke-width=\"3\" opacity=\"{opacity}\"/>");
            }
        }

        // Wires (red positive, black negative)
        svg.AddLine(X - 15, Y + 35, X - 15, Y + 60, "#F44336", 4, "round");
        svg.AddLine(X + 15, Y + 35, X + 15, Y + 60, "#212121", 4, "round");

        // Connection points
        svg.AddCircle(X - 15, Y + 60, 5, "#F44336");
        svg.AddCircle(X + 15, Y + 60, 5, "#212121");

        // Polarity labels
        svg.AddText(X - 15, Y + 75, "+", 12, "#F44336", "middle", "bold");
        svg.AddText(X + 15, Y + 75, "-", 12, "#212121", "middle", "bold");

        // Label
        if (!string.IsNullOrEmpty(Label))
        {
            svg.AddText(X, Y - 55, Label, 14, "#333", "middle", "bold");
        }
    }
}
