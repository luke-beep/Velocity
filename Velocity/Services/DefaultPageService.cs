using Velocity.Contracts.Services;

namespace Velocity.Services;

public class DefaultPageService : IDefaultPageService
{
    private const string SettingsKey = "DefaultPage";

    public PageType DefaultPage { get;
        private set; } = PageType.Home;

    private readonly ILocalSettingsService _localSettingsService;

    public event Action? DefaultPageChanged;

    public DefaultPageService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        DefaultPage = await LoadDefaultPageFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetDefaultPageAsync(PageType page)
    {
        DefaultPage = page;
        DefaultPageChanged?.Invoke();
        await SaveDefaultPageInSettingsAsync(page);
    }

    public async Task<PageType> LoadDefaultPageFromSettingsAsync()
    {
        var pageName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
        return Enum.TryParse(pageName, out PageType pageType) ? pageType : PageType.Home;
    }

    private async Task SaveDefaultPageInSettingsAsync(PageType page)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, page.ToString());
    }

    public enum PageType
    {
        Home,
        Tweaks,
        Games,
        Apps,
        Health,
        Updates,
        Services,
        Debug,
        Settings
    }
}