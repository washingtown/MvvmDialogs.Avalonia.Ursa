using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ursa.Controls;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaOverlayMessageBoxDialogFactory : UrsaMessageBoxDialogFactoryBase
{
    public UrsaOverlayMessageBoxDialogFactory(IDialogFactory? chain) : base(chain)
    {
    }

    protected override async Task<bool?> ShowMessageBoxDialogAsync(IView? owner, MessageBoxSettings settings)
    {
        string message = settings.Content;
        string title = settings.Title;
        string? hostId = null;
        int? toplevelHashCode = owner?.RefObj.GetHashCode();
        if (owner?.ViewModel is IDialogContextOwner { DialogContext: { } context })
        {
            hostId = context.HostId;
            toplevelHashCode = context.ToplevelHashCode;
        }

        var result = await MessageBox.ShowOverlayAsync(
            message,
            title,
            hostId,
            toplevelHashCode: toplevelHashCode
        );
        
        return result switch
        {
            MessageBoxResult.Yes => true,
            MessageBoxResult.OK => true,
            MessageBoxResult.No => false,
            MessageBoxResult.Cancel => null,
            _ => null
        };
            
        
    }
}