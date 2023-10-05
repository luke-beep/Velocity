using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NLog;
using Velocity.Activation;
using Velocity.Contracts.Services;
using Velocity.Helpers;
using Velocity.Models;
using Velocity.Views;
using static NLog.LogManager;

namespace Velocity.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IBackdropSelectorService _backdropSelectorService;
    private readonly IDefaultPageService _defaultPageService;
    private readonly ILogService _logService;
    private readonly IAlwaysOnTopService _alwaysOnTopService;
    private readonly Logger _logger = GetCurrentClassLogger();

    private UIElement? _shell;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, IThemeSelectorService themeSelectorService, IBackdropSelectorService backdropSelectorService, ILogService logService, IDefaultPageService defaultPageService, IAlwaysOnTopService alwaysOnTopService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
        _backdropSelectorService = backdropSelectorService;
        _logService = logService;
        _defaultPageService = defaultPageService;
        _alwaysOnTopService = alwaysOnTopService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        await InitializeAsync();

        if (App.MainWindow.Content == null)
        {
            _shell = App.GetService<ShellPage>();
            App.MainWindow.Content = _shell ?? new Frame();
        }

        await HandleActivationAsync(activationArgs);

        App.MainWindow.Activate();

        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        await _backdropSelectorService.InitializeAsync().ConfigureAwait(false);
        await _defaultPageService.InitializeAsync().ConfigureAwait(false);
        await _logService.InitializeAsync().ConfigureAwait(false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await LogExtension.Log(_logger, LogLevel.Info, "Application started.", LogEvent.EventIds.Startup, null);
        await Task.CompletedTask;
    }
}
