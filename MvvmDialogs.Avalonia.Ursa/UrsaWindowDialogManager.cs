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

public class UrsaWindowDialogManager : UrsaDialogManagerBase
{

    public UrsaWindowDialogManager(
        IViewLocator? viewLocator = null,
        ILogger<UrsaWindowDialogManager>? logger = null,
        IDispatcher? dispatcher = null,
        IWindowFactory? windowFactory = null
        ):
        base(
            viewLocator ?? new ViewLocatorBase(),
            new DialogFactory().AddUrsaWindowMessageBox(),
            logger)
    {
    }

    protected override bool IsDesignMode => Design.IsDesignMode;
    private UrsaWindowViewWrapper NewWrapper() => new UrsaWindowViewWrapper(WindowFactory);
    
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
    
    
    
}