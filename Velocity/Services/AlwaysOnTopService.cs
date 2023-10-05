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

    public AlwaysOnTop IsAlwaysOnTop
    {
        get;
        private set;
    }

    public async Task InitializeAsync()
    {
        IsAlwaysOnTop = await LoadAlwaysOnTopFromSettingsAsync();
        await Task.CompletedTask;
    }

    private async Task SaveAlwaysOnTopInSettings(AlwaysOnTop value)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, value.ToString());
    }

    private async Task<AlwaysOnTop> LoadAlwaysOnTopFromSettingsAsync()
    {
        var alwaysOnTop = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
        return Enum.TryParse(alwaysOnTop, out AlwaysOnTop alwaysOnTopResult) ? alwaysOnTopResult : AlwaysOnTop.Disabled;
    }

    public async Task InitializeAlwaysOnTop()
    {
        var alwaysOnTop = await LoadAlwaysOnTopFromSettingsAsync();
        await SetAlwaysOnTopAsync(alwaysOnTop);
    }

    public async Task SetAlwaysOnTopAsync(AlwaysOnTop value)
    {
        if (IsAlwaysOnTop == value)
        {
            return;
        }

        IsAlwaysOnTop = value;
        App.MainWindow.IsAlwaysOnTop = value switch
        {
            AlwaysOnTop.Enabled => true,
            AlwaysOnTop.Disabled => false,
            _ => App.MainWindow.IsAlwaysOnTop
        };
        AlwaysOnTopChanged?.Invoke();
        await SaveAlwaysOnTopInSettings(value);
        await LogExtension.Log(Logger, LogLevel.Info, $"Always On Top set to {value}", EventIds.DebugInformation, null);
    }

    public enum AlwaysOnTop
    {
        Enabled,
        Disabled
    }
}