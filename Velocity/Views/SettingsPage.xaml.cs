using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Velocity.Contracts.Services;
using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class SettingsPage : Page
{
    private readonly ILocalSettingsService _localSettingsService = App.GetService<ILocalSettingsService>() ?? throw new InvalidOperationException("Failed to get ILocalSettingsService service.");

    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>() ?? throw new InvalidOperationException("Failed to get SettingsViewModel service.");
        InitializeComponent();
    }

    private void OpenSettings(object sender, RoutedEventArgs routedEventArgs)
    {
        _localSettingsService.OpenSettings();
    }

    private void EnableAlwaysOnTop(object sender, RoutedEventArgs e)
    {
        ViewModel.SwitchAlwaysOnTopCommand.Execute(true);
    }

    private void DisableAlwaysOnTop(object sender, RoutedEventArgs e)
    {
        ViewModel.SwitchAlwaysOnTopCommand.Execute(false);
    }
}