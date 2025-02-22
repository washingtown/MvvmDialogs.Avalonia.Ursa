namespace MvvmDialogs.Avalonia.Ursa;

public class DialogContext
{
    public DialogContext(int toplevelHashCode,string? hostId=null)
    {
        ToplevelHashCode = toplevelHashCode;
        HostId = hostId;
    }
    public int ToplevelHashCode { get; set; }

    public string? HostId { get; set; }
}