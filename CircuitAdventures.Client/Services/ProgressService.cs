using CircuitAdventures.Client.Models;

namespace CircuitAdventures.Client.Services;

/// <summary>
/// Service for managing user progress through levels
/// </summary>
public class ProgressService
{
    private readonly UserService _userService;
    private readonly CertificateService _certificateService;

    public ProgressService(UserService userService, CertificateService certificateService)
    {
        _userService = userService;
        _certificateService = certificateService;
    }

    /// <summary>
    /// Mark a level as completed with the specified number of stars
    /// </summary>
    public async Task<bool> CompleteLevelAsync(int levelId, int stars)
    {
        var user = _userService.CurrentUser;
        if (user == null) return false;

        // Add to completed levels if not already completed
        if (!user.CompletedLevels.Contains(levelId))
        {
            user.CompletedLevels.Add(levelId);
        }

        // Update stars (only if better than previous)
        if (!user.StarsEarned.ContainsKey(levelId) || user.StarsEarned[levelId] < stars)
        {
            user.StarsEarned[levelId] = stars;
        }

        // Advance to next level if this was the current level
        if (levelId == user.CurrentLevel && levelId < 100)
        {
            user.CurrentLevel = levelId + 1;
        }

        // Check if milestone reached for certificate
        if (levelId % 10 == 0 && levelId <= 100)
        {
            await _certificateService.AwardCertificateAsync(levelId, user.Name);
        }

        await _userService.SaveUserDataAsync();
        await _userService.UpdateStreakAsync();

        return true;
    }

    /// <summary>
    /// Check if a level is unlocked for the user
    /// </summary>
    public bool IsLevelUnlocked(int levelId)
    {
        var user = _userService.CurrentUser;
        if (user == null) return false;

        // Levels are unlocked sequentially: all completed levels + current level
        return levelId <= user.CurrentLevel;
    }

    /// <summary>
    /// Get the total number of stars earned
    /// </summary>
    public int GetTotalStars()
    {
        var user = _userService.CurrentUser;
        if (user == null) return 0;

        return user.StarsEarned.Values.Sum();
    }

    /// <summary>
    /// Get completion percentage (0-100)
    /// </summary>
    public double GetCompletionPercentage()
    {
        var user = _userService.CurrentUser;
        if (user == null) return 0;

        return (user.CompletedLevels.Count / 100.0) * 100;
    }

    /// <summary>
    /// Get stars earned for a specific level
    /// </summary>
    public int GetStarsForLevel(int levelId)
    {
        var user = _userService.CurrentUser;
        if (user == null) return 0;

        return user.StarsEarned.TryGetValue(levelId, out var stars) ? stars : 0;
    }

    /// <summary>
    /// Check if a level is completed
    /// </summary>
    public bool IsLevelCompleted(int levelId)
    {
        var user = _userService.CurrentUser;
        if (user == null) return false;

        return user.CompletedLevels.Contains(levelId);
    }

    /// <summary>
    /// Add a badge to the user's collection
    /// </summary>
    public async Task AwardBadgeAsync(string badgeId)
    {
        var user = _userService.CurrentUser;
        if (user == null) return;

        if (!user.Badges.Contains(badgeId))
        {
            user.Badges.Add(badgeId);
            await _userService.SaveUserDataAsync();
        }
    }

    /// <summary>
    /// Check if user has earned a specific badge
    /// </summary>
    public bool HasBadge(string badgeId)
    {
        var user = _userService.CurrentUser;
        if (user == null) return false;

        return user.Badges.Contains(badgeId);
    }
}
