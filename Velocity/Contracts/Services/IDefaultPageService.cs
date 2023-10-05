using Microsoft.UI.Xaml.Controls;
using static Velocity.Services.DefaultPageService;

namespace Velocity.Contracts.Services;

public interface IDefaultPageService
{
    PageType DefaultPage
    {
        get;
    }
    Task SetDefaultPageAsync(PageType backdrop);

    event Action DefaultPageChanged;
    Task InitializeAsync();
    Task<PageType> LoadDefaultPageFromSettingsAsync();
}