using Ursa.Controls;

namespace MvvmDialogs.Avalonia.Ursa;

/// <summary>
/// Context used by DialogManager to find owner Window and Ursa Overlay.
/// </summary>
public class DialogContext
{
    public DialogContext(int toplevelHashCode,string? hostId=null)
    {
        ToplevelHashCode = toplevelHashCode;
        HostId = hostId;
    }
    /// <summary>
    /// HashCode of the owner Toplevel object.
    /// </summary>
    public int ToplevelHashCode { get; set; }
    /// <summary>
    /// Host ID of <see cref="OverlayDialogHost"/>
    /// </summary>
    public string? HostId { get; set; }
}