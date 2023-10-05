using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class AppsPage : Page
{
    public AppsViewModel ViewModel
    {
        get;
    }

    public AppsPage()
    {
        ViewModel = App.GetService<AppsViewModel>();
        InitializeComponent();
    }
}
