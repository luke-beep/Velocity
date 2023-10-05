using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class HomePage : Page
{
    private const string ButtonBackgroundPointerOverResource = "ButtonBackgroundPointerOver";
    private const string ButtonBackgroundResource = "ButtonBackground";

    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }

    private void HoverOnButtonBorder(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = (SolidColorBrush)Application.Current.Resources[ButtonBackgroundPointerOverResource] ??
                                throw new InvalidOperationException(
                                    $"Failed to get {ButtonBackgroundPointerOverResource} resource.");
        }
    }


    private void HoverOffButtonBorder(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = (SolidColorBrush)Application.Current.Resources[ButtonBackgroundResource] ??
                                throw new InvalidOperationException(
                                    $"Failed to get {ButtonBackgroundResource} resource.");
        }
    }

    private async Task<BitmapImage?> SetUserProfilePicture()
    {
        var currentUser = User.FindAllAsync().GetResults().FirstOrDefault();
        var picture = await currentUser?.GetPictureAsync(UserPictureSize.Size64x64);
        if (picture != null)
        {
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(await picture.OpenReadAsync());
            return bitmapImage;
        }
        return null;
    }

    private async Task<string?> SetUserProfileName()
    {
        User? currentUser = null;
        foreach (var user in (await User.FindAllAsync()))
        {
            currentUser = user;
            break;
        }

        if (currentUser != null)
        {
            var displayName = await currentUser.GetPropertyAsync(KnownUserProperties.DisplayName);
            return displayName.ToString();
        }

        return null;
    }
}