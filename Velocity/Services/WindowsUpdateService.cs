using NLog;
using Velocity.Contracts.Services;
using Velocity.Helpers;
using Velocity.Models;

namespace Velocity.Services;
public class WindowsUpdateService : IWindowsUpdateService
{
    private readonly dynamic? _updateSession;
    private readonly dynamic? _updateSearcher;
    private dynamic? _searchResult;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public WindowsUpdateService()
    {
        _updateSession = Activator.CreateInstance(Type.GetTypeFromProgID("Microsoft.Update.Session") ?? throw new InvalidOperationException());
        if (_updateSession == null)
        {
            return;
        }

        _updateSession.ClientApplicationID = "Velocity";
        _updateSearcher = _updateSession.CreateUpdateSearcher();
    }

    public async Task<IEnumerable<WindowsUpdate>> GetAvailableUpdatesAsync()
    {
        try
        {
            var updates = new List<WindowsUpdate>();
            await Task.Run(() =>
            {
                if (_updateSearcher != null)
                {
                    _searchResult = _updateSearcher.Search("IsInstalled=0");
                }

                if (_searchResult == null)
                {
                    return;
                }

                for (var i = 0; i < _searchResult.Updates.Count; ++i)
                {
                    var update = _searchResult.Updates.Item(i);
                    updates.Add(new WindowsUpdate
                    {
                        Title = update.Title,
                        Description = update.Description,
                        IsDownloaded = update.IsDownloaded,
                        IsInstalled = update.IsInstalled
                    });
                }
            });
            await LogExtension.Log(Logger, LogLevel.Info, $"Successfully retrieved available Windows updates.", LogEvent.EventIds.AvailableUpdatesRetrieved, null);
            return updates;
        }
        catch (Exception ex)
        {
            await LogExtension.Log(Logger, LogLevel.Error, ex.ToString(), LogEvent.EventIds.AvailableUpdatesFailedToRetrieve, ex);
            throw;
        }

    }

    public async Task DownloadUpdateAsync(WindowsUpdate update)
    {
        await Task.Run(() =>
        {
            LogExtension.Log(Logger, LogLevel.Info, $"Downloading {update.Title}", LogEvent.EventIds.UpdateDownloadStarted, null);
            dynamic updatesToDownload = Activator.CreateInstance(Type.GetTypeFromProgID("Microsoft.Update.UpdateColl"));
            if (updatesToDownload != null)
            {
                updatesToDownload.Add(update);
                if (_updateSession != null)
                {
                    var downloader = _updateSession.CreateUpdateDownloader();
                    downloader.Updates = updatesToDownload;
                    var downloadResult = downloader.Download();

                    if (downloadResult.ResultCode != 2)
                    {
                        LogExtension.Log(Logger, LogLevel.Error, $"Download failed with result code: {downloadResult.ResultCode}", LogEvent.EventIds.UpdateDownloadFailed, null);
                    }
                    LogExtension.Log(Logger, LogLevel.Info, $"Successfully downloaded {update.Title} with result code: {downloadResult.ResultCode}", LogEvent.EventIds.UpdateDownloadCompleted, null);
                }
            }
        });
    }

    public async Task InstallUpdateAsync(WindowsUpdate update)
    {
        await Task.Run(() =>
        {
            LogExtension.Log(Logger, LogLevel.Info, $"Installing {update.Title}", LogEvent.EventIds.UpdateInstallStarted, null);
            dynamic updatesToInstall = Activator.CreateInstance(Type.GetTypeFromProgID("Microsoft.Update.UpdateColl") ?? throw new InvalidOperationException()) ?? throw new InvalidOperationException();
            if (updatesToInstall != null)
            {
                updatesToInstall.Add(update);
                if (_updateSession != null)
                {
                    var installer = _updateSession.CreateUpdateInstaller();
                    installer.Updates = updatesToInstall;
                    var installResult = installer.Install();

                    if (installResult.ResultCode != 2)
                    {
                        LogExtension.Log(Logger, LogLevel.Error, $"Install failed with result code: {installResult.ResultCode}", LogEvent.EventIds.UpdateInstallFailed, null);
                    }
                    LogExtension.Log(Logger, LogLevel.Info, $"Successfully installed {update.Title} with result code: {installResult.ResultCode}", LogEvent.EventIds.UpdateInstallCompleted, null);
                }
            }
        });
    }
}