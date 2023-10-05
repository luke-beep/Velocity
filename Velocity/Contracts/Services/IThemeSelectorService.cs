using Microsoft.UI.Xaml;

namespace Velocity.Contracts.Services;

public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
    }
    event Action ThemeChanged;
    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();
}
