using static Velocity.Services.BackdropSelectorService;

namespace Velocity.Contracts.Services;

public interface IBackdropSelectorService
{
    BackdropType Backdrop
    {
        get;
    }
    Task SetBackdropAsync(BackdropType backdrop);
    event Action BackdropChanged;
    Task InitializeAsync();
}