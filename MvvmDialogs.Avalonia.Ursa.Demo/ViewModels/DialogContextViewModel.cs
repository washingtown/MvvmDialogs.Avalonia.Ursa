using Irihi.Avalonia.Shared.Contracts;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public abstract class DialogContextViewModel : ViewModelBase, IDialogContextOwner
{
    private readonly IDialogContextProvider _dialogContextProvider;

    public DialogContextViewModel(IDialogContextProvider dialogContextProvider)
    {
        _dialogContextProvider = dialogContextProvider;
    }
    public DialogContext? DialogContext
    {
        get => _dialogContextProvider.DialogContext;
        set
        {
            if(value!=null)
                _dialogContextProvider.DialogContext = value;
        }
    }
    
    
}