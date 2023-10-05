using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Velocity.Contracts.Services;
using Velocity.Contracts.ViewModels;
using Velocity.Models;

namespace Velocity.ViewModels;

public partial class UpdatesViewModel : ObservableRecipient, INavigationAware
{
    private readonly IWindowsUpdateService _windowsUpdateService;

    [ObservableProperty]
    private WindowsUpdate? _selected;

    public ObservableCollection<WindowsUpdate> AvailableUpdates
    {
        get;
        set;
    } = new();

    public UpdatesViewModel(IWindowsUpdateService windowsUpdateService)
    {
        _windowsUpdateService = windowsUpdateService;
    }

    public Task OnNavigatedTo(object parameter)
    {
        return Task.CompletedTask;
    }

    public async Task LoadUpdatesAsync()
    {
        AvailableUpdates.Clear();

        var updates = await _windowsUpdateService.GetAvailableUpdatesAsync();

        foreach (var update in updates)
        {
            AvailableUpdates.Add(update);
        }
    }

    public async Task DownloadAndInstallUpdateAsync()
    {
        if (Selected is not null)
        {
            await _windowsUpdateService.DownloadUpdateAsync(Selected);
            await _windowsUpdateService.InstallUpdateAsync(Selected);
        }
    }

    public async Task DownloadUpdateAsync()
    {
        if (Selected is not null)
        {
            await _windowsUpdateService.DownloadUpdateAsync(Selected);
        }
    }

    public async Task InstallUpdateAsync()
    {
        if (Selected is not null)
        {
            await _windowsUpdateService.InstallUpdateAsync(Selected);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (AvailableUpdates.Any())
        {
            Selected ??= AvailableUpdates.First();
        }
    }
}
