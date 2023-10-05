using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;
using NLog;
using Velocity.Contracts.Services;
using Velocity.Helpers;
using Velocity.Models;

namespace Velocity.Services;

public class BackdropSelectorService : IBackdropSelectorService
{
    private const string SettingsKey = "AppRequestedBackdrop";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public event Action? BackdropChanged;

    public BackdropType Backdrop
    {
        get;
        private set;
    } = BackdropType.Mica;

    private readonly ILocalSettingsService _localSettingsService;
    public BackdropSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        Backdrop = await LoadBackdropFromSettingsAsync();
        await Task.CompletedTask;
    }


    public async Task SetBackdropAsync(BackdropType type)
    {
        try
        {
            Backdrop = type;
            switch (type)
            {
                case BackdropType.Mica:
                    App.MainWindow.SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.Base };
                    await LogExtension.Log(logger: Logger, LogLevel.Info, $"Switched to {type}", LogEvent.EventIds.BackDropChanged, null);
                    break;
                case BackdropType.MicaAlt:
                    App.MainWindow.SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.BaseAlt };
                    await LogExtension.Log(logger: Logger, LogLevel.Info, $"Switched to {type}", LogEvent.EventIds.BackDropChanged, null);
                    break;
                case BackdropType.DesktopAcrylic:
                    App.MainWindow.SystemBackdrop = new DesktopAcrylicBackdrop();
                    await LogExtension.Log(logger: Logger, LogLevel.Info, $"Switched to {type}", LogEvent.EventIds.BackDropChanged, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            BackdropChanged?.Invoke();
            await SaveBackdropInSettingsAsync(type);
        }
        catch (Exception e)
        {
            await LogExtension.Log(logger: Logger, LogLevel.Error, e.Message, LogEvent.EventIds.BackDropFailedToChange, e);
            throw;
        }
    }
    private async Task SaveBackdropInSettingsAsync(BackdropType type)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, type.ToString());
    }

    private async Task<BackdropType> LoadBackdropFromSettingsAsync()
    {
        var backDrop = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
        return Enum.TryParse(backDrop, out BackdropType backdropType) ? backdropType : BackdropType.Mica;
    }
    
    public enum BackdropType
    {
        Mica,
        MicaAlt,
        DesktopAcrylic
    }
}   