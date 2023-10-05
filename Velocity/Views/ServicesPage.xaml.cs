using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class ServicesPage : Page
{
    public ServicesViewModel ViewModel
    {
        get;
    }

    public ServicesPage()
    {
        ViewModel = App.GetService<ServicesViewModel>();
        InitializeComponent();
    }
}
