using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Threading;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Microsoft.Extensions.Logging;
using Ursa.Controls;
using MessageBoxButton = HanumanInstitute.MvvmDialogs.FrameworkDialogs.MessageBoxButton;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaWindowDialogManager : DialogManagerBase<ContentControl>
{
    private readonly IDispatcher _dispatcher;
    private readonly Func<Window>? _windowFactory;

    public UrsaWindowDialogManager(
        IViewLocator? viewLocator = null,
        IDialogFactory? dialogFactory = null,
        ILogger<DialogManager>? logger = null,
        IDispatcher? dispatcher = null,
        Func<Window>? windowFactory = null
        ):
        base(
            viewLocator ?? new ViewLocatorBase(),
            dialogFactory ?? new DialogFactory(),
            logger)
    {
        _dispatcher = dispatcher ?? Dispatcher.UIThread;
        _windowFactory = windowFactory;
    }

    protected override bool IsDesignMode => Design.IsDesignMode;
    private static IEnumerable<Window> Windows =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows ?? Array.Empty<Window>();
    private UrsaWindowViewWrapper NewWrapper() => new UrsaWindowViewWrapper(_windowFactory);
    
    protected override IView CreateWrapper(INotifyPropertyChanged viewModel, ViewDefinition viewDef)
    {
        var wrapper = NewWrapper();
        wrapper.Initialize(viewModel, viewDef);
        return wrapper;
    }

    protected override IView AsWrapper(ContentControl view)
    {
        var wrapper = NewWrapper();
        wrapper.InitializeExisting((INotifyPropertyChanged)view.DataContext!, view);
        return wrapper;
    }
    
    
    public override IView? FindViewByViewModel(INotifyPropertyChanged viewModel)
    {
        return FindWindowByViewModel(viewModel).AsWindowWrapper(_windowFactory);
    }

    private Window? FindWindowByViewModel(INotifyPropertyChanged viewModel)
    {
        Window? window = viewModel switch
        {
            _ => Windows.FirstOrDefault(x => ReferenceEquals(viewModel, x.DataContext))
        };
        return window;
    }
    
    public override IView? GetMainWindow()
    {
        var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow.AsWindowWrapper(_windowFactory);
        return mainWindow;
    }

    public override IView? GetDummyWindow()
    {
        var parent = new Window()
        {
            Height = 1,
            Width = 1,
            SystemDecorations = SystemDecorations.None,
            ShowInTaskbar = false,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            Background = Brushes.Transparent
        };
        parent.Show();
        return parent.AsWindowWrapper();
    }
    
    /// <inheritdoc />
    protected override void Dispatch(Action action)
    {
        if (_dispatcher.CheckAccess())
        {
            action();
        }
        else
        {
            _dispatcher.Post(action, DispatcherPriority.Render);
        }
    }

    /// <inheritdoc />
    protected override Task<T> DispatchAsync<T>(Func<T> action) =>
        //_dispatcher.CheckAccess() ? Task.FromResult(action()) : _dispatcher.InvokeAsync(action, DispatcherPriority.Render);
        _dispatcher.CheckAccess() ? Task.FromResult(action()) : DispatchWithResult(action);
    /// <summary>
    /// Work-around for missing interface member in Avalonia v11-preview1.
    /// </summary>
    private Task<T> DispatchWithResult<T>(Func<T> action)
    {
        var tcs = new TaskCompletionSource<T>();
        _dispatcher.Post(
            () =>
            {
                tcs.SetResult(action());
            },
            DispatcherPriority.Render);
        return tcs.Task;
    }
    
}