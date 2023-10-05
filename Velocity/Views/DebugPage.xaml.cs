using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class DebugPage : Page
{
    public DebugViewModel ViewModel
    {
        get;
    }

    public DebugPage()
    {
        ViewModel = App.GetService<DebugViewModel>();
        InitializeComponent();
    }

    private void GenerateSampleError(object sender, RoutedEventArgs e)
    {
        ViewModel.GenerateSampleError().ConfigureAwait(false);
    }
    private void RefreshLogs(object sender, RoutedEventArgs e)
    {
        ViewModel.RefreshLogs().ConfigureAwait(false);
    }

    private void FixLogs(object sender, RoutedEventArgs e)
    {
        ViewModel.FixLogs().ConfigureAwait(false);
    }

    private void OpenLogFolder(object sender, RoutedEventArgs e)
    {
        ViewModel.OpenLogFolder();
    }
}
