using Velocity.Contracts.Services;
using Velocity.ViewModels;
using TeachingTip = Microsoft.UI.Xaml.Controls.TeachingTip;

namespace Velocity.Services;

public class IntroductionService : IIntroductionService
{
    private readonly INavigationService _navigationService;
    private Dictionary<string, TeachingTip> _tipsDictionary = new();

    public IntroductionService(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public void HandleTipActionButtonClick(TeachingTip sender, object args)
    {
        sender.IsOpen = false;
        var nextPage = GetNextPage(sender);
        _navigationService.NavigateTo(nextPage, args);
        ShowTipForPage(nextPage);
    }

    public void HandleTipActionButtonClickNoNav(TeachingTip sender, object args)
    {
        sender.IsOpen = false;
        var nextPage = GetNextPage(sender);
        ShowTipForPage(nextPage);
    }

    public void HandleTipBackButtonClick(TeachingTip sender, object args)
    {
        var formerPage = GetFormerPage(sender);
        _navigationService.NavigateTo(formerPage, args);
        ShowTipForPage(formerPage);
    }

    public void HandleTipBackButtonClickNoNav(TeachingTip sender, object args)
    {
        var formerPage = GetFormerPage(sender);
        ShowTipForPage(formerPage);
    }


    public void CloseTip(TeachingTip sender, object args)
    {
        sender.IsOpen = false;
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName, args);
    }
    public void CloseTipNoNav(TeachingTip sender, object args)
    {
        sender.IsOpen = false;
    }

    public void ShowTipForPage(string? page)
    {
        if (page != null && _tipsDictionary.TryGetValue(page, out var tipToShow))
        {
            tipToShow.IsOpen = true;
        }
    }

    private static readonly Dictionary<string, string> NextPageMapping = new()
    {
        { typeof(HomeViewModel).FullName!, typeof(TweaksViewModel).FullName! },
        { typeof(TweaksViewModel).FullName!, typeof(GamesViewModel).FullName! },
        { typeof(GamesViewModel).FullName!, typeof(AppsViewModel).FullName! },
        { typeof(AppsViewModel).FullName!, typeof(HealthViewModel).FullName! },
        { typeof(HealthViewModel).FullName!, typeof(UpdatesViewModel).FullName! },
        { typeof(UpdatesViewModel).FullName!, typeof(ServicesViewModel).FullName! },
        { typeof(ServicesViewModel).FullName!, typeof(DebugViewModel).FullName! },
        { typeof(DebugViewModel).FullName!, typeof(SettingsViewModel).FullName! },
    };

    private static readonly Dictionary<string, string> FormerPageMapping = new()
    {
        { typeof(TweaksViewModel).FullName!, typeof(HomeViewModel).FullName! },
        { typeof(GamesViewModel).FullName!, typeof(TweaksViewModel).FullName! },
        { typeof(AppsViewModel).FullName!, typeof(GamesViewModel).FullName! },
        { typeof(HealthViewModel).FullName!, typeof(AppsViewModel).FullName! },
        { typeof(UpdatesViewModel).FullName!, typeof(HealthViewModel).FullName! },
        { typeof(ServicesViewModel).FullName!, typeof(UpdatesViewModel).FullName! },
        { typeof(DebugViewModel).FullName!, typeof(ServicesViewModel).FullName! },
        { typeof(SettingsViewModel).FullName!, typeof(DebugViewModel).FullName! },
    };

    private string? GetNextPage(TeachingTip sender)
    {
        var key = _tipsDictionary.FirstOrDefault(kvp => sender == kvp.Value).Key;
        return NextPageMapping.TryGetValue(key, out var nextPage) ? nextPage : string.Empty;
    }

    private string? GetFormerPage(TeachingTip sender)
    {
        var key = _tipsDictionary.FirstOrDefault(kvp => sender == kvp.Value).Key;
        return FormerPageMapping.TryGetValue(key, out var formerPage) ? formerPage : string.Empty;
    }

    public void Initialize(Dictionary<string, TeachingTip> tipsDictionary)
    {
        _tipsDictionary = tipsDictionary;
    }
}