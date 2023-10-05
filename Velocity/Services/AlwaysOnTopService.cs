using NLog;
using Velocity.Contracts.Services;
using Velocity.Helpers;
using static Velocity.Models.LogEvent;

namespace Velocity.Services;

public class AlwaysOnTopService : IAlwaysOnTopService
{
    private const string SettingsKey = "AppAlwaysOnTop";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public event Action? AlwaysOnTopChanged;

    
    private readonly ILocalSettingsService _localSettingsService;
    public AlwaysOnTopService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public bool IsAlwaysOnTop
    {
        get;
        private set;
    }

    public async Task InitializeAsync()
    {
        IsAlwaysOnTop = await LoadAlwaysOnTopFromSettingsAsync();
        App.MainWindow.IsAlwaysOnTop = IsAlwaysOnTop;
    }

    private async Task SaveAlwaysOnTopInSettings(bool value)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, value);
    }

    private async Task<bool> LoadAlwaysOnTopFromSettingsAsync()
    {
        return await _localSettingsService.ReadSettingAsync<bool>(SettingsKey);
    }

    public async Task SetAlwaysOnTopAsync(bool value)
    {
        if (IsAlwaysOnTop == value)
        {
            return;
        }

        IsAlwaysOnTop = value;
        App.MainWindow.IsAlwaysOnTop = value;
        AlwaysOnTopChanged?.Invoke();
        await SaveAlwaysOnTopInSettings(value);

        await LogExtension.Log(Logger, LogLevel.Info, $"Always On Top set to {value}", EventIds.DebugInformation, null);
    }

}