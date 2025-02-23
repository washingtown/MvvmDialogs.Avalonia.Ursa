using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Threading;
using HanumanInstitute.MvvmDialogs;
using Microsoft.Extensions.Logging;

namespace MvvmDialogs.Avalonia.Ursa;

public abstract class UrsaDialogManagerBase : DialogManagerBase<ContentControl>
{
    protected IDispatcher Dispatcher { get; init; }
    protected IWindowFactory? WindowFactory { get; init; }
    protected UrsaDialogManagerBase(
        IViewLocator viewLocator, 
        IDialogFactory dialogFactory, 
        ILogger<UrsaDialogManagerBase>? logger,
        IDispatcher? dispatcher = null,
        IWindowFactory? windowFactory = null
        ) : base(viewLocator, dialogFactory, logger)
    {
        Dispatcher = dispatcher ?? global::Avalonia.Threading.Dispatcher.UIThread;
        WindowFactory = windowFactory;
    }
    
    protected override bool IsDesignMode => Design.IsDesignMode;
    
    protected static IEnumerable<Window> Windows =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows ?? Array.Empty<Window>();

    /// <inheritdoc />
    public override IView? GetMainWindow()
    {
        var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow.AsWindowWrapper(WindowFactory);
        return mainWindow;
    }
    /// <inheritdoc />
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
    public override IView? FindViewByViewModel(INotifyPropertyChanged viewModel)
    {
        return FindWindowByViewModel(viewModel).AsWindowWrapper(WindowFactory);
    }
    
    private Window? FindWindowByViewModel(INotifyPropertyChanged viewModel)
    {
        Window? window = viewModel switch
        {
            IDialogContextOwner dc=>Windows.FirstOrDefault(x => x.GetHashCode()==dc.DialogContext?.ToplevelHashCode),
            _ => Windows.FirstOrDefault(x => ReferenceEquals(viewModel, x.DataContext))
        };
        return window;
    }
    /// <inheritdoc />
    protected override void Dispatch(Action action)
    {
        if (Dispatcher.CheckAccess())
        {
            action();
        }
        else
        {
            Dispatcher.Post(action, DispatcherPriority.Render);
        }
    }

    /// <inheritdoc />
    protected override Task<T> DispatchAsync<T>(Func<T> action) =>
        //Dispatcher.CheckAccess() ? Task.FromResult(action()) : Dispatcher.InvokeAsync(action, DispatcherPriority.Render);
        Dispatcher.CheckAccess() ? Task.FromResult(action()) : DispatchWithResult(action);
    /// <summary>
    /// Work-around for missing interface member in Avalonia v11-preview1.
    /// </summary>
    private Task<T> DispatchWithResult<T>(Func<T> action)
    {
        var tcs = new TaskCompletionSource<T>();
        Dispatcher.Post(
            () =>
            {
                tcs.SetResult(action());
            },
            DispatcherPriority.Render);
        return tcs.Task;
    }
}