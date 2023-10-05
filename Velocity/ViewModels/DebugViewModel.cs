using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using NLog;
using Velocity.Contracts.Services;
using Velocity.Contracts.ViewModels;
using Velocity.Helpers;
using Velocity.Models;

namespace Velocity.ViewModels;

public partial class DebugViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<LogEvent> Logs { get; } = new();

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly ILogService _logService;

    public DebugViewModel(ILogService logService)
    {
        _logService = logService;
    }

    public Task OnNavigatedTo(object parameter)
    {
        LoadLogs().ConfigureAwait(false);
        return Task.CompletedTask;
    }

    public void OnNavigatedFrom()
    {
        Logs.Clear();
    }

    private static async IAsyncEnumerable<LogEvent> ParseLogFileAsync(string filePath)
    {
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream);

        while (await streamReader.ReadLineAsync() is { } line)
        {
            LogEvent? log = null;

            try
            {
                log = JsonConvert.DeserializeObject<LogEvent>(line);
            }
            catch (Exception ex)
            {
                await LogExtension.Log(Logger, LogLevel.Error, ex.ToString(), LogEvent.EventIds.IoError, null);
            }

            if (log != null)
            {
                yield return log;
            }
        }
    }

    private async Task LoadLogs()
    {
        var filePath = await _logService.GetFilePath();
        if (string.IsNullOrEmpty(filePath))
        {
            await LogExtension.Log(Logger, LogLevel.Info, $"Failed to load in `{filePath}`", LogEvent.EventIds.IoError, null);
            return;
        }

        await foreach (var log in ParseLogFileAsync(filePath))
        {
            Logs.Add(log);
        }
    }
    public Task FixLogs()
    {
        _logService.SetupNLog();
        return Task.CompletedTask;
    }

    public async Task RefreshLogs()
    {
        Logs.Clear();
        await LoadLogs();
    }

    public void OpenLogFolder()
    {
        _logService.OpenLogFolder();
    }

    public async Task GenerateSampleError()
    {
        await LogExtension.Log(Logger, LogLevel.Info, $"Sample Error!", LogEvent.EventIds.DebugInformation, null);
        await RefreshLogs();
    }
}
