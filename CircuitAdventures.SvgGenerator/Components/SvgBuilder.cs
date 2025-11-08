using System.Text;
using System.Xml;

namespace CircuitAdventures.SvgGenerator.Components;

/// <summary>
/// Helper class for building SVG elements
/// </summary>
public class SvgBuilder
{
    private readonly StringBuilder _svg = new();
    private readonly int _width;
    private readonly int _height;

    public SvgBuilder(int width = 800, int height = 600)
    {
        _width = width;
        _height = height;
    }

    public void StartSvg()
    {
        _svg.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{_width}\" height=\"{_height}\" viewBox=\"0 0 {_width} {_height}\">");

        // Add definitions section
        _svg.AppendLine("  <defs>");

        // Add grid pattern
        _svg.AppendLine("    <pattern id=\"grid\" width=\"40\" height=\"40\" patternUnits=\"userSpaceOnUse\">");
        _svg.AppendLine("      <path d=\"M 40 0 L 0 0 0 40\" fill=\"none\" stroke=\"#e8f4f8\" stroke-width=\"1\"/>");
        _svg.AppendLine("    </pattern>");

        // Add glow filter for highlights
        _svg.AppendLine("    <filter id=\"glow\">");
        _svg.AppendLine("      <feGaussianBlur stdDeviation=\"4\" result=\"coloredBlur\"/>");
        _svg.AppendLine("      <feMerge>");
        _svg.AppendLine("        <feMergeNode in=\"coloredBlur\"/>");
        _svg.AppendLine("        <feMergeNode in=\"SourceGraphic\"/>");
        _svg.AppendLine("      </feMerge>");
        _svg.AppendLine("    </filter>");

        // Add arrow marker for connection indicators
        _svg.AppendLine("    <marker id=\"arrowhead\" markerWidth=\"10\" markerHeight=\"7\" refX=\"9\" refY=\"3.5\" orient=\"auto\">");
        _svg.AppendLine("      <polygon points=\"0 0, 10 3.5, 0 7\" fill=\"#FF9800\" />");
        _svg.AppendLine("    </marker>");

        _svg.AppendLine("  </defs>");

        // Background
        _svg.AppendLine($"  <rect width=\"{_width}\" height=\"{_height}\" fill=\"#f8fcff\"/>");

        // Grid
        _svg.AppendLine($"  <rect width=\"{_width}\" height=\"{_height}\" fill=\"url(#grid)\"/>");
    }

    public void EndSvg()
    {
        _svg.AppendLine("</svg>");
    }

    public void AddRectangle(double x, double y, double width, double height, string fill = "#ccc", string? stroke = null, double strokeWidth = 1, double rx = 0)
    {
        var strokeAttr = stroke != null ? $" stroke=\"{stroke}\" stroke-width=\"{strokeWidth}\"" : "";
        var rxAttr = rx > 0 ? $" rx=\"{rx}\"" : "";
        _svg.AppendLine($"  <rect x=\"{x}\" y=\"{y}\" width=\"{width}\" height=\"{height}\" fill=\"{fill}\"{strokeAttr}{rxAttr}/>");
    }

    public void AddCircle(double cx, double cy, double r, string fill = "#ccc", string? stroke = null, double strokeWidth = 1)
    {
        var strokeAttr = stroke != null ? $" stroke=\"{stroke}\" stroke-width=\"{strokeWidth}\"" : "";
        _svg.AppendLine($"  <circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" fill=\"{fill}\"{strokeAttr}/>");
    }

    public void AddLine(double x1, double y1, double x2, double y2, string stroke = "#333", double strokeWidth = 2, string? strokeLinecap = null, string? markerEnd = null)
    {
        var linecapAttr = strokeLinecap != null ? $" stroke-linecap=\"{strokeLinecap}\"" : "";
        var markerAttr = markerEnd != null ? $" marker-end=\"url(#{markerEnd})\"" : "";
        _svg.AppendLine($"  <line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" stroke=\"{stroke}\" stroke-width=\"{strokeWidth}\"{linecapAttr}{markerAttr}/>");
    }

    public void AddPath(string d, string? fill = null, string? stroke = null, double strokeWidth = 2, string? filter = null)
    {
        var fillAttr = fill != null ? $" fill=\"{fill}\"" : " fill=\"none\"";
        var strokeAttr = stroke != null ? $" stroke=\"{stroke}\" stroke-width=\"{strokeWidth}\"" : "";
        var filterAttr = filter != null ? $" filter=\"url(#{filter})\"" : "";
        _svg.AppendLine($"  <path d=\"{d}\"{fillAttr}{strokeAttr}{filterAttr}/>");
    }

    public void AddText(double x, double y, string text, int fontSize = 16, string fill = "#333", string textAnchor = "middle", string? fontWeight = null)
    {
        var weightAttr = fontWeight != null ? $" font-weight=\"{fontWeight}\"" : "";
        _svg.AppendLine($"  <text x=\"{x}\" y=\"{y}\" font-size=\"{fontSize}\" fill=\"{fill}\" text-anchor=\"{textAnchor}\" font-family=\"Arial, sans-serif\"{weightAttr}>{text}</text>");
    }

    public void AddGroup(string content, string? transform = null, string? filter = null)
    {
        var transformAttr = transform != null ? $" transform=\"{transform}\"" : "";
        var filterAttr = filter != null ? $" filter=\"url(#{filter})\"" : "";
        _svg.AppendLine($"  <g{transformAttr}{filterAttr}>");
        _svg.Append(content);
        _svg.AppendLine("  </g>");
    }

    public void AddHighlightBox(double x, double y, double width, double height, string color = "#FFC107")
    {
        // Add a glowing highlight box
        _svg.AppendLine($"  <rect x=\"{x}\" y=\"{y}\" width=\"{width}\" height=\"{height}\" fill=\"none\" stroke=\"{color}\" stroke-width=\"3\" stroke-dasharray=\"8,4\" rx=\"8\" filter=\"url(#glow)\"/>");
    }

    public void AddArrow(double x1, double y1, double x2, double y2, string color = "#FF9800", string? label = null)
    {
        _svg.AppendLine($"  <line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" stroke=\"{color}\" stroke-width=\"3\" marker-end=\"url(#arrowhead)\"/>");

        if (label != null)
        {
            // Add label at midpoint
            var midX = (x1 + x2) / 2;
            var midY = (y1 + y2) / 2;
            _svg.AppendLine($"  <text x=\"{midX}\" y=\"{midY - 10}\" font-size=\"14\" fill=\"{color}\" text-anchor=\"middle\" font-family=\"Arial, sans-serif\" font-weight=\"bold\">{label}</text>");
        }
    }

    public string Build()
    {
        return _svg.ToString();
    }

    public void AddRaw(string svgContent)
    {
        _svg.AppendLine(svgContent);
    }
}
