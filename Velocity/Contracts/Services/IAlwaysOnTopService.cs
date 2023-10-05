using static Velocity.Services.AlwaysOnTopService;

namespace Velocity.Contracts.Services;

public interface IAlwaysOnTopService
{
    AlwaysOnTop IsAlwaysOnTop
    {
        get;
    }
    Task SetAlwaysOnTopAsync(AlwaysOnTop value);
    Task InitializeAlwaysOnTop();
    event Action AlwaysOnTopChanged;
    Task InitializeAsync();

}