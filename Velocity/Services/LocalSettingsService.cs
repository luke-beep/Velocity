using Windows.Storage;
using Microsoft.Extensions.Options;
using Velocity.Contracts.Services;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Helpers;
using Velocity.Helpers;
using Velocity.Models;

namespace Velocity.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string DefaultApplicationDataFolder = "Velocity\\ApplicationData";
    private const string DefaultLocalSettingsFile = "LocalSettings.json";

    private readonly IFileService _fileService;
    private readonly LocalSettingsOptions _options;

    private readonly string _localApplicationData =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private readonly string _applicationDataFolder;
    private readonly string _localsettingsFile;

    private IDictionary<string, object> _settings;

    private bool _isInitialized;

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData, DefaultApplicationDataFolder);
        _localsettingsFile = DefaultLocalSettingsFile;
        _settings = new Dictionary<string, object>();
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings = await Task.Run(() =>
                            _fileService.Read<IDictionary<string, object>>(_applicationDataFolder,
                                _localsettingsFile)) ??
                        new Dictionary<string, object>();

            _isInitialized = true;
        }
    }

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        if (RuntimeHelper.IsMsix)
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }
        else
        {
            await InitializeAsync();

            if (_settings != null && _settings.TryGetValue(key, out var obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        if (RuntimeHelper.IsMsix)
        {
            ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
        }
        else
        {
            await InitializeAsync();

            _settings[key] = await Json.StringifyAsync(value);

            await Task.Run(() => _fileService.Save(_applicationDataFolder, _localsettingsFile, _settings));
        }
    }

    public async Task OpenSettings()
    {
        await FileExtension.OpenFolder(_applicationDataFolder);
    }
}