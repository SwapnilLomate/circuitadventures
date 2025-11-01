using CircuitAdventures.Client.Models;

namespace CircuitAdventures.Client.Services;

/// <summary>
/// Service for managing certificates
/// </summary>
public class CertificateService
{
    private readonly UserService _userService;
    private readonly List<CertificateDesign> _certificateDesigns;

    public CertificateService(UserService userService)
    {
        _userService = userService;
        _certificateDesigns = InitializeCertificateDesigns();
    }

    /// <summary>
    /// Award a certificate to the user for reaching a milestone
    /// </summary>
    public async Task AwardCertificateAsync(int milestoneLevel, string userName)
    {
        var user = _userService.CurrentUser;
        if (user == null) return;

        // Check if certificate already exists
        if (user.Certificates.Any(c => c.Level == milestoneLevel))
        {
            return;
        }

        var design = _certificateDesigns.FirstOrDefault(d => d.Level == milestoneLevel);
        if (design == null) return;

        var certificate = new Certificate
        {
            Id = Guid.NewGuid().ToString(),
            Name = design.Name,
            Level = milestoneLevel,
            EarnedDate = DateTime.Now,
            UserName = userName
        };

        user.Certificates.Add(certificate);
        await _userService.SaveUserDataAsync();
    }

    /// <summary>
    /// Get all certificate designs
    /// </summary>
    public List<CertificateDesign> GetAllCertificateDesigns()
    {
        return _certificateDesigns;
    }

    /// <summary>
    /// Get certificate design for a specific milestone level
    /// </summary>
    public CertificateDesign? GetCertificateDesign(int milestoneLevel)
    {
        return _certificateDesigns.FirstOrDefault(d => d.Level == milestoneLevel);
    }

    /// <summary>
    /// Check if user has earned a certificate for a specific milestone
    /// </summary>
    public bool HasCertificate(int milestoneLevel)
    {
        var user = _userService.CurrentUser;
        if (user == null) return false;

        return user.Certificates.Any(c => c.Level == milestoneLevel);
    }

    /// <summary>
    /// Get all earned certificates
    /// </summary>
    public List<Certificate> GetEarnedCertificates()
    {
        var user = _userService.CurrentUser;
        if (user == null) return new List<Certificate>();

        return user.Certificates;
    }

    /// <summary>
    /// Initialize all certificate designs (10 milestones)
    /// </summary>
    private List<CertificateDesign> InitializeCertificateDesigns()
    {
        return new List<CertificateDesign>
        {
            new() { Id = "cert-10", Name = "Spark Starter Certificate", Level = 10, Description = "For igniting your journey into electronics", BorderColor = "#3B82F6", IconSymbol = "⚡", BackgroundColor = "#EFF6FF", Template = "template-1" },
            new() { Id = "cert-20", Name = "Current Explorer Certificate", Level = 20, Description = "For exploring the flow of electricity", BorderColor = "#8B5CF6", IconSymbol = "🔌", BackgroundColor = "#F5F3FF", Template = "template-2" },
            new() { Id = "cert-30", Name = "Circuit Apprentice Certificate", Level = 30, Description = "For mastering basic circuits", BorderColor = "#10B981", IconSymbol = "🔧", BackgroundColor = "#ECFDF5", Template = "template-3" },
            new() { Id = "cert-40", Name = "Voltage Voyager Certificate", Level = 40, Description = "For voyaging through power and energy", BorderColor = "#F59E0B", IconSymbol = "⚙️", BackgroundColor = "#FFFBEB", Template = "template-4" },
            new() { Id = "cert-50", Name = "Motion Magician Certificate", Level = 50, Description = "For bringing circuits to life with movement", BorderColor = "#EF4444", IconSymbol = "🎯", BackgroundColor = "#FEF2F2", Template = "template-5" },
            new() { Id = "cert-60", Name = "Light Illuminator Certificate", Level = 60, Description = "For brilliantly mastering LED circuits", BorderColor = "#FBBF24", IconSymbol = "💡", BackgroundColor = "#FFFBEB", Template = "template-6" },
            new() { Id = "cert-70", Name = "Power Pioneer Certificate", Level = 70, Description = "For pioneering advanced electronics", BorderColor = "#06B6D4", IconSymbol = "🚀", BackgroundColor = "#ECFEFF", Template = "template-7" },
            new() { Id = "cert-80", Name = "Innovation Inventor Certificate", Level = 80, Description = "For inventing creative solutions", BorderColor = "#EC4899", IconSymbol = "🔬", BackgroundColor = "#FDF2F8", Template = "template-8" },
            new() { Id = "cert-90", Name = "Circuit Champion Certificate", Level = 90, Description = "For championing complex circuits", BorderColor = "#8B5CF6", IconSymbol = "🏆", BackgroundColor = "#FAF5FF", Template = "template-9" },
            new() { Id = "cert-100", Name = "Master Electrician Certificate", Level = 100, Description = "For mastering all of CircuitAdventures", BorderColor = "#F59E0B", IconSymbol = "👑", BackgroundColor = "#FFFBEB", Template = "template-10" }
        };
    }
}
