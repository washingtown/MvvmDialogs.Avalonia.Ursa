using Avalonia.Threading;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.Logging;

namespace MvvmDialogs.Avalonia.Ursa;
/// <summary>
/// DialogService to provide Ursa window dialogs.
/// </summary>
public class UrsaWindowDialogService : DialogServiceBase
{
    public UrsaWindowDialogService(
        IViewLocator viewLocator,
        ILogger<UrsaWindowDialogManager>? logger = null,
        IDispatcher? dispatcher = null,
        IWindowFactory? windowFactory = null, 
        Func<Type, object?>? viewModelFactory=null) : 
        base(
            new UrsaWindowDialogManager(
                viewLocator,
                logger,
                dispatcher,
                windowFactory
                ), 
            viewModelFactory)
    {
        
    }
}