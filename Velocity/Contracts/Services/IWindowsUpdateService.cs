using Velocity.Models;

namespace Velocity.Contracts.Services;

public interface IWindowsUpdateService
{
    Task<IEnumerable<WindowsUpdate>> GetAvailableUpdatesAsync();
    Task DownloadUpdateAsync(WindowsUpdate update);
    Task InstallUpdateAsync(WindowsUpdate update);
}
