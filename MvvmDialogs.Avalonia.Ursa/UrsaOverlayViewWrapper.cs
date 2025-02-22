using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HanumanInstitute.MvvmDialogs;
using Ursa.Controls;
using Ursa.EventArgs;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaOverlayViewWrapper : IView
{
    public void Initialize(INotifyPropertyChanged viewModel, ViewDefinition viewDef)
    {
        var view = viewDef.Create();
        InitializeExisting(viewModel, view);
    }

    public void InitializeExisting(INotifyPropertyChanged viewModel, object view)
    {
        switch (view)
        {
            case ContentControl control:
            {
                Ref = new CustomDialogControl();
                Ref.Content = control;
                break;
            }
            default:
            {
                throw new InvalidOperationException(
                    $"The type of created view must be {typeof(UserControl)}, but got {view.GetType().Name}");
            }
        }
        ViewModel = viewModel;
    }

    public void Show(IView? owner)
    {
        if(owner == null)
            throw new InvalidOperationException("Overlay dialog must have a owner");
        if (owner.RefObj is not Window window)
            throw new InvalidOperationException(
                $"The type of {nameof(IView.RefObj)} must be {typeof(Window)}, but got {owner?.RefObj.GetType().Name}");
        int hashCode=window.GetHashCode();
        string? hostId = null;
        if (owner.ViewModel is IDialogContextOwner
            {
                DialogContext: {} context
            })
        {
            hostId = context.HostId;
            hashCode = context.ToplevelHashCode;
        }

        OverlayDialogOptions options = new() { TopLevelHashCode = hashCode };
        OverlayDialog.ShowCustom(Ref, ViewModel, hostId, options);
    }

    public Task ShowDialogAsync(IView owner)
    {
        if(owner == null)
            throw new InvalidOperationException("Overlay dialog must have a owner");
        if (owner.RefObj is not Window window)
            throw new InvalidOperationException(
                $"The type of {nameof(IView.RefObj)} must be {typeof(Window)}, but got {owner?.RefObj.GetType().Name}");
        int hashCode=window.GetHashCode();
        string? hostId = null;
        if (owner.ViewModel is IDialogContextOwner
            {
                DialogContext: {} context
            })
        {
            hostId = context.HostId;
            hashCode = context.ToplevelHashCode;
        }

        OverlayDialogOptions options = new() { TopLevelHashCode = hashCode };
        return OverlayDialog.ShowCustomModal<bool>(Ref, ViewModel, hostId, options);
    }

    public void Activate()
    {
        
    }

    public void Close()
    {
        Ref.Close();
    }

    public DialogControlBase Ref { get; set; } = default!;
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
        add
        {
            if (value == null) return;
            var handler = new EventHandler<RoutedEventArgs>(value.Invoke);
            _loadedHandlers.Add(value, handler);
            Ref.Loaded += handler;
        }
        remove
        {
            if (value == null) return;
            Ref.Loaded -= _loadedHandlers[value];
            _loadedHandlers.Remove(value);
        }
    }

    private readonly Dictionary<EventHandler, EventHandler<RoutedEventArgs>> _loadedHandlers = new();
    
    public event EventHandler<CancelEventArgs>? Closing;
    /// <summary>
    /// Fired when the dialog is closed.
    /// </summary>
    public event EventHandler? Closed
    {
        add
        {
            if (value == null) return;
            var handler = new EventHandler<ResultEventArgs>(value.Invoke);
            _closedHandlers.Add(value, handler);
            Ref.Closed += handler;
        }
        remove
        {
            if (value == null) return;
            Ref.Closed -= _closedHandlers[value];
            _closedHandlers.Remove(value);
        }
    }
    private readonly Dictionary<EventHandler, EventHandler<ResultEventArgs>> _closedHandlers = new();

}