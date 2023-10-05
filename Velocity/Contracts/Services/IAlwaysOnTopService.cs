namespace Velocity.Contracts.Services;

public interface IAlwaysOnTopService
{
    bool IsAlwaysOnTop
    {
        get;
    }
    Task SetAlwaysOnTopAsync(bool value);
    event Action AlwaysOnTopChanged;
    Task InitializeAsync();

}