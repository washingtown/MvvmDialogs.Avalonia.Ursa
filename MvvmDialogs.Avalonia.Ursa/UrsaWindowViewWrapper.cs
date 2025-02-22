using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HanumanInstitute.MvvmDialogs;
using Ursa.Controls;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaWindowViewWrapper : IView
{
    public UrsaWindowViewWrapper(IWindowFactory? windowFactory = null)
    {
        _windowFactory = windowFactory;
    }

    private readonly IWindowFactory? _windowFactory;
    /// <inheritdoc />
    public void Initialize(INotifyPropertyChanged viewModel, ViewDefinition viewDef)
    {
        var view = viewDef.Create();
        InitializeExisting(viewModel, view);
    }
    /// <inheritdoc />
    public void InitializeExisting(INotifyPropertyChanged viewModel, object view)
    {
        switch (view)
        {
            case Window window:
            {
                Ref = window;
                break;
            }
            case ContentControl control:
            {
                Ref = _windowFactory?.CreateWindow() ?? new DialogWindow() { CanResize = true };
                Ref.Content = control;
                break;
            }
            default:
            {
                throw new InvalidOperationException(
                    $"The type of created view must be {typeof(Window)}, but got {view.GetType().Name}");
            }
        }
        
        ViewModel = viewModel;
        if (ViewModel is IDialogContextOwner dialogContextOwner)
        {
            dialogContextOwner.DialogContext = new DialogContext(Ref.GetHashCode(), null);
        }
    }
    
    
    public void Show(IView? owner)
    {
        var own = owner?.RefObj switch
        {
            Window window => window,
            null => null,
            _ => throw new InvalidOperationException(
                $"The type of {nameof(IView.RefObj)} must be {typeof(Window)}, but got {owner?.RefObj.GetType().Name}")
        };
        SetMainWindowIfEmpty(Ref);
        if (own == null)
        {
            Ref.Show();
        }
        else
        {
            Ref.Icon ??= own.Icon;
            Ref.Show(own);
        }
    }

    public Task ShowDialogAsync(IView owner)
    {
        var own = owner?.RefObj switch
        {
            Window window => window,
            _ => throw new InvalidOperationException(
                $"The type of {nameof(IView.RefObj)} must be {typeof(Window)}, but got {owner?.RefObj.GetType().Name}")
        };
        SetMainWindowIfEmpty(Ref);
        Ref.Icon ??= own.Icon;
        return Ref.ShowDialog<bool>(own);
    }

    public void Activate() => Ref.Activate();

    public void Close() => Ref.Close();
    
    private void SetMainWindowIfEmpty(Window? window)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: null } desktop)
        {
            desktop.MainWindow = window;
        }
    }
    
    /// <summary>
    /// Gets the Window reference held by this class.
    /// </summary>
    public Window Ref { get; private set; } = default!;

    public object RefObj => Ref;
    
    /// <inheritdoc />
    public INotifyPropertyChanged ViewModel
    {
        get => (INotifyPropertyChanged)Ref.DataContext!;
        set => Ref.DataContext = value;
    }
    /// <inheritdoc />

    public bool IsEnabled
    {
        get => Ref.IsEnabled;
        set => Ref.IsEnabled = value;
        
    }
    /// <inheritdoc />
    public bool IsVisible => Ref.IsVisible;
    /// <inheritdoc />
    public bool ClosingConfirmed { get; set; }
    /// <inheritdoc />
    public event EventHandler? Loaded
    {
        add => Ref.Opened += value;
        remove => Ref.Opened -= value;
    }
    /// <summary>
    /// Fired when the window is closing.
    /// </summary>
    public event EventHandler<CancelEventArgs>? Closing
    {
        add
        {
            if (value == null) return;
            var handler = new EventHandler<WindowClosingEventArgs>(value.Invoke);
            _closingHandlers.Add(value, handler);
            Ref.Closing += handler;
        }
        remove
        {
            if (value == null) return;
            Ref.Closing -= _closingHandlers[value];
            _closingHandlers.Remove(value);
        }
    }
    private readonly Dictionary<EventHandler<CancelEventArgs>, EventHandler<WindowClosingEventArgs>> _closingHandlers = new();

    /// <summary>
    /// Fired when the window is closed.
    /// </summary>
    public event EventHandler? Closed
    {
        add => Ref.Closed += value;
        remove => Ref.Closed -= value;
    }
}