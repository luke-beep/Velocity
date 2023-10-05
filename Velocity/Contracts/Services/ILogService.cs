namespace Velocity.Contracts.Services;
public interface ILogService
{
    Task OpenLogFolder();
    Task SetupNLog();
    Task InitializeAsync();
    Task<string> GetFilePath();
    Task<string> GetFolderPath();
}
