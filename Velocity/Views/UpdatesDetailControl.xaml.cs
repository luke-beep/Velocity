using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Velocity.Models;

namespace Velocity.Views;

public sealed partial class UpdatesDetailControl : UserControl
{
    public static readonly DependencyProperty WindowsUpdateProperty = DependencyProperty.Register("WindowsUpdate",
        typeof(WindowsUpdate), typeof(UpdatesDetailControl),
        new PropertyMetadata(null, OnWindowsUpdatePropertyChanged));

    public WindowsUpdate WindowsUpdate
    {
        get => (WindowsUpdate)GetValue(WindowsUpdateProperty);
        set => SetValue(WindowsUpdateProperty, value);
    }
    public UpdatesDetailControl()
    {
        InitializeComponent();
    }

    private static void OnWindowsUpdatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UpdatesDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
