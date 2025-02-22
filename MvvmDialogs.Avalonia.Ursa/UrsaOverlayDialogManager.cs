using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Threading;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.Logging;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaOverlayDialogManager : UrsaDialogManagerBase
{
    private readonly IDispatcher _dispatcher;
    public UrsaOverlayDialogManager(
        IViewLocator viewLocator, 
        ILogger<UrsaOverlayDialogManager>? logger,
        IDispatcher? dispatcher = null
        ) : base(
        viewLocator, 
        new DialogFactory().AddUrsaOverlayMessageBox(), 
        logger)
    {
        _dispatcher = dispatcher ?? global::Avalonia.Threading.Dispatcher.UIThread;
    }
    protected override bool IsDesignMode => Design.IsDesignMode;
    
    protected override IView CreateWrapper(INotifyPropertyChanged viewModel, ViewDefinition viewDef)
    {
        var wrapper = new UrsaOverlayViewWrapper();
        wrapper.Initialize(viewModel, viewDef);
        return wrapper;
    }

    protected override IView AsWrapper(ContentControl view)
    {
        var wrapper = new UrsaOverlayViewWrapper();
        wrapper.InitializeExisting((INotifyPropertyChanged)view.DataContext!, view);
        return wrapper;
    }
}