using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using Velocity.Contracts.Services;
using Velocity.ViewModels;
using Velocity.Views;

namespace Velocity.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<HomeViewModel, HomePage>();
        Configure<TweaksViewModel, TweaksPage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<UpdatesViewModel, UpdatesPage>();
        Configure<DebugViewModel, DebugPage>();
        Configure<AppsViewModel, AppsPage>();
        Configure<GamesViewModel, GamesPage>();
        Configure<ServicesViewModel, ServicesPage>();
        Configure<HealthViewModel, HealthPage>();
    }

    public Type? GetPageType(string? key)
    {
        Type? pageType = null;
        lock (_pages)
        {
            if (key != null && !_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<TVm, TV>()
        where TVm : ObservableObject
        where TV : Page
    {
        lock (_pages)
        {
            var key = typeof(TVm).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(TV);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
