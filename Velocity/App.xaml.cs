using LogLevel = NLog.LogLevel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using NLog;
using NLog.Extensions.Logging;

using Velocity.Activation;
using Velocity.Contracts.Services;
using Velocity.Core.Contracts.Services;
using Velocity.Core.Services;
using Velocity.Helpers;
using Velocity.Models;
using Velocity.Services;
using Velocity.ViewModels;
using Velocity.Views;

namespace Velocity;

public partial class App : Application
{
    private IHost Host
    {
        get;
    }


    public static T GetService<T>()
        where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow
    {
        get;
    } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get;
        set;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddNLog();

        }).
        ConfigureServices((context, services) =>
        {
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IBackdropSelectorService, BackdropSelectorService>();
            services.AddSingleton<IDefaultPageService, DefaultPageService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<INavigationViewService, NavigationViewService>();
            services.AddSingleton<IBackdropSelectorService, BackdropSelectorService>();
            services.AddSingleton<IWindowsUpdateService, WindowsUpdateService>();
            services.AddSingleton<IIntroductionService, IntroductionService>();
            services.AddSingleton<IAlwaysOnTopService, AlwaysOnTopService>();

            services.AddSingleton<IFileService, FileService>();

            services.AddTransient<HealthViewModel>();
            services.AddTransient<HealthPage>();
            services.AddTransient<ServicesViewModel>();
            services.AddTransient<ServicesPage>();
            services.AddTransient<GamesViewModel>();
            services.AddTransient<GamesPage>();
            services.AddTransient<AppsViewModel>();
            services.AddTransient<AppsPage>();
            services.AddTransient<DebugViewModel>();
            services.AddTransient<DebugPage>();
            services.AddTransient<UpdatesViewModel>();
            services.AddTransient<UpdatesPage>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<TweaksViewModel>();
            services.AddTransient<TweaksPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        })
        .Build();
        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        var logger = LogManager.GetCurrentClassLogger();
        LogExtension.Log(logger, LogLevel.Error, e.Exception.Message, LogEvent.EventIds.UnhandledException, e.Exception);
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await GetService<IActivationService>().ActivateAsync(args);
    }
}
