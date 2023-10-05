using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class HealthPage : Page
{
    public HealthViewModel ViewModel
    {
        get;
    }

    public HealthPage()
    {
        ViewModel = App.GetService<HealthViewModel>();
        InitializeComponent();
    }
}
