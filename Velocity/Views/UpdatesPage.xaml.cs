using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class UpdatesPage : Page
{
    public UpdatesViewModel ViewModel
    {
        get;
    }

    public UpdatesPage()
    {
        ViewModel = App.GetService<UpdatesViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }

    private async void ScanButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadUpdatesAsync();
    }

    private async void DownloadAndInstallButton_Click(object sender, RoutedEventArgs e)
    {
        await ViewModel.DownloadAndInstallUpdateAsync();
    }
}
