using CircuitAdventures.Client.Models;

namespace CircuitAdventures.Client.Services;

/// <summary>
/// Service for managing user data and state
/// </summary>
public class UserService
{
    private readonly LocalStorageService _localStorageService;
    private UserData? _currentUser;

    public event Action? OnUserDataChanged;

    public UserService(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    /// <summary>
    /// Get the current user data (cached)
    /// </summary>
    public UserData? CurrentUser => _currentUser;

    /// <summary>
    /// Initialize user service by loading data from localStorage
    /// </summary>
    public async Task InitializeAsync()
    {
        _currentUser = await _localStorageService.LoadUserDataAsync();
        NotifyUserDataChanged();
    }

    /// <summary>
    /// Check if user has been onboarded (has name entered)
    /// </summary>
    public async Task<bool> IsUserOnboardedAsync()
    {
        if (_currentUser == null)
        {
            await InitializeAsync();
        }
        return _currentUser != null && !string.IsNullOrEmpty(_currentUser.Name);
    }

    /// <summary>
    /// Create a new user with provided name and optional starting level
    /// </summary>
    public async Task CreateUserAsync(string name, string? avatar = null, int startingLevel = 1)
    {
        _currentUser = new UserData
        {
            Name = name,
            Avatar = avatar,
            FirstVisit = DateTime.Now,
            LastVisit = DateTime.Now,
            CurrentLevel = startingLevel
        };

        await SaveUserDataAsync();
    }

    /// <summary>
    /// Update current user data
    /// </summary>
    public async Task UpdateUserAsync(UserData userData)
    {
        _currentUser = userData;
        await SaveUserDataAsync();
    }

    /// <summary>
    /// Save current user data to localStorage
    /// </summary>
    public async Task SaveUserDataAsync()
    {
        if (_currentUser != null)
        {
            await _localStorageService.SaveUserDataAsync(_currentUser);
            NotifyUserDataChanged();
        }
    }

    /// <summary>
    /// Update streak information
    /// </summary>
    public async Task UpdateStreakAsync()
    {
        if (_currentUser == null) return;

        var today = DateTime.Now.Date;
        var lastActive = _currentUser.Streak.LastActiveDate.Date;
        var daysSinceLastActive = (today - lastActive).Days;

        if (daysSinceLastActive == 0)
        {
            // Already active today, no change
            return;
        }
        else if (daysSinceLastActive == 1)
        {
            // Consecutive day, increment streak
            _currentUser.Streak.Current++;
            if (_currentUser.Streak.Current > _currentUser.Streak.Longest)
            {
                _currentUser.Streak.Longest = _currentUser.Streak.Current;
            }
        }
        else
        {
            // Streak broken, reset to 1
            _currentUser.Streak.Current = 1;
        }

        _currentUser.Streak.LastActiveDate = today;
        await SaveUserDataAsync();
    }

    /// <summary>
    /// Add a note for a specific level
    /// </summary>
    public async Task AddNoteAsync(int levelId, string note)
    {
        if (_currentUser == null) return;

        _currentUser.Notes[levelId] = note;
        await SaveUserDataAsync();
    }

    /// <summary>
    /// Reset all user data (use with caution)
    /// </summary>
    public async Task ResetUserDataAsync()
    {
        await _localStorageService.ClearDataAsync();
        _currentUser = null;
        NotifyUserDataChanged();
    }

    private void NotifyUserDataChanged()
    {
        OnUserDataChanged?.Invoke();
    }
}
