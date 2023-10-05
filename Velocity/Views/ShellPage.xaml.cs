using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Velocity.Contracts.Services;
using Velocity.Helpers;
using Velocity.ViewModels;

namespace Velocity.Views;

public sealed partial class ShellPage : Page
{
    private readonly INavigationService _navigationService;
    private readonly IIntroductionService _introductionService;

    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel, INavigationService navigationService,
        IIntroductionService introductionService)
    {
        ViewModel = viewModel;
        _navigationService = navigationService;
        _introductionService = introductionService;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_ActivatedAsync;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
        Loaded += OnLoaded;
        _navigationService.Navigated += (sender, args) =>
        {
            var pageType = args.SourcePageType.FullName;
            _introductionService.ShowTipForPage(pageType);
        };
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
        StartIntroduction();
    }


    private async void MainWindow_ActivatedAsync(object sender, WindowActivatedEventArgs args)
    {
        var resource = args.WindowActivationState == WindowActivationState.Deactivated
            ? "WindowCaptionForegroundDisabled"
            : "WindowCaptionForeground";

        AppTitleBarText.Foreground = (SolidColorBrush)Application.Current.Resources[resource];
        App.AppTitlebar = AppTitleBarText;
    }


    private void NavigationViewControl_DisplayModeChanged(NavigationView sender,
        NavigationViewDisplayModeChangedEventArgs args) =>
        AppTitleBar.Margin = new Thickness
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender,
        KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void StartIntroduction()
    {
        var tipsDictionary = new Dictionary<string, TeachingTip>
        {
            { typeof(HomeViewModel).FullName!, HomeTip },
            { typeof(TweaksViewModel).FullName!, TweaksTip },
            { typeof(GamesViewModel).FullName!, GamesTip },
            { typeof(AppsViewModel).FullName!, AppsTip },
            { typeof(HealthViewModel).FullName!, HealthTip },
            { typeof(UpdatesViewModel).FullName!, UpdatesTip },
            { typeof(ServicesViewModel).FullName!, ServicesTip },
            { typeof(DebugViewModel).FullName!, DebugTip },
            { typeof(SettingsViewModel).FullName!, SettingsTip }
        };
        _introductionService.Initialize(tipsDictionary);
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName);
        AppOverlayGrid.Visibility = Visibility.Visible;
        InitialTip.IsOpen = true;
        InitialTip.TailVisibility = TeachingTipTailVisibility.Collapsed;
        EndTip.TailVisibility = TeachingTipTailVisibility.Collapsed;
        InitialTip.ShouldConstrainToRootBounds = true;
    }

    private void GoNext(TeachingTip sender, object args)
    {
        _introductionService.CloseTip(sender, args);
        _introductionService.HandleTipActionButtonClick(sender, args);
    }

    private void GoNextNoNav(TeachingTip sender, object args)
    {
        _introductionService.CloseTipNoNav(sender, args);
        _introductionService.HandleTipActionButtonClickNoNav(sender, args);
    }

    private void GoNextAlt(TeachingTip sender, object args)
    {
        CloseTipNoNav(sender, args);
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName);
        _introductionService.ShowTipForPage(typeof(HomeViewModel).FullName);
    }

    private void GoBack(TeachingTip sender, object args) => _introductionService.HandleTipBackButtonClick(sender, args);

    private void GoBackNoNav(TeachingTip sender, object args)
    {
        _introductionService.HandleTipBackButtonClickNoNav(sender, args);
        AppOverlayGrid.Visibility = Visibility.Visible;
    }

    private void GoToInitialTip(TeachingTip sender, object args)
    {
        sender.IsOpen = false;
        InitialTip.IsOpen = true;
    }

    private void Skip(TeachingTip sender, object args)
    {
        _introductionService.CloseTip(sender, args);
        AppOverlayGrid.Visibility = Visibility.Collapsed;
    }

    private void CloseTipNoNav(TeachingTip sender, object args) => _introductionService.CloseTipNoNav(sender, args);

    private void GoToSettings(TeachingTip sender, object args)
    {
        sender.IsOpen = false;
        SettingsTip.IsOpen = true;
    }

    private void GoToEnd(TeachingTip sender, object args)
    {
        sender.IsOpen = false;
        EndTip.IsOpen = true;
    }

    private void Finish(TeachingTip sender, object args)
    {
        _introductionService.CloseTip(sender, args);
        AppOverlayGrid.Visibility = Visibility.Collapsed;
    }
}