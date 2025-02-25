using Avalonia.Threading;
using HanumanInstitute.MvvmDialogs;
using Microsoft.Extensions.Logging;

namespace MvvmDialogs.Avalonia.Ursa;

/// <summary>
/// DialogService to provide Ursa overlay dialogs.
/// </summary>
public class UrsaOverlayDialogService : DialogServiceBase
{
    public UrsaOverlayDialogService(
        IViewLocator viewLocator,
        ILogger<UrsaOverlayDialogManager>? logger = null,
        IDispatcher? dispatcher = null,
        IWindowFactory? windowFactory = null,
        Func<Type, object?>? viewModelFactory = null) :
        base(
            new UrsaOverlayDialogManager(
                viewLocator,
                logger,
                dispatcher
            ),
            viewModelFactory)
    {

    }
}