using Blazored.LocalStorage;
using CircuitAdventures.Client.Models;

namespace CircuitAdventures.Client.Services;

/// <summary>
/// Service for managing localStorage operations with Blazored.LocalStorage
/// </summary>
public class LocalStorageService
{
    private readonly ILocalStorageService _localStorage;
    private const string STORAGE_KEY = "circuitadventures-user";
    private const string BACKUP_KEY = "circuitadventures-backup";

    public LocalStorageService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    /// <summary>
    /// Save user data to localStorage and create a backup
    /// </summary>
    public async Task SaveUserDataAsync(UserData data)
    {
        try
        {
            data.LastVisit = DateTime.Now;
            await _localStorage.SetItemAsync(STORAGE_KEY, data);
            // Also save backup
            await _localStorage.SetItemAsync(BACKUP_KEY, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving user data: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Load user data from localStorage
    /// </summary>
    public async Task<UserData?> LoadUserDataAsync()
    {
        try
        {
            return await _localStorage.GetItemAsync<UserData>(STORAGE_KEY);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user data: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Recover user data from backup
    /// </summary>
    public async Task<UserData?> RecoverFromBackupAsync()
    {
        try
        {
            return await _localStorage.GetItemAsync<UserData>(BACKUP_KEY);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error recovering from backup: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Clear all user data (use with caution)
    /// </summary>
    public async Task ClearDataAsync()
    {
        try
        {
            await _localStorage.RemoveItemAsync(STORAGE_KEY);
            await _localStorage.RemoveItemAsync(BACKUP_KEY);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing data: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Check if user data exists in localStorage
    /// </summary>
    public async Task<bool> HasUserDataAsync()
    {
        try
        {
            return await _localStorage.ContainKeyAsync(STORAGE_KEY);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking user data: {ex.Message}");
            return false;
        }
    }
}
