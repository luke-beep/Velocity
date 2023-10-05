using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class TweaksPage : Page
{
    public TweaksViewModel ViewModel
    {
        get;
    }

    public TweaksPage()
    {
        ViewModel = App.GetService<TweaksViewModel>();
        InitializeComponent();
    }
}
