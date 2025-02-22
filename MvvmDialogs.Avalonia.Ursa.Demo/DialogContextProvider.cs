namespace MvvmDialogs.Avalonia.Ursa.Demo;

public class DialogContextProvider: IDialogContextProvider
{
    public DialogContext DialogContext { get; set; } = new DialogContext(0, null);
}