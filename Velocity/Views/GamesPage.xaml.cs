using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class GamesPage : Page
{
    public GamesViewModel ViewModel
    {
        get;
    }

    public GamesPage()
    {
        ViewModel = App.GetService<GamesViewModel>();
        InitializeComponent();
    }
}
