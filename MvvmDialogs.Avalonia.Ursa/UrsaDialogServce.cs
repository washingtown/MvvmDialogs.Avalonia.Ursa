using HanumanInstitute.MvvmDialogs;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaDialogServce : DialogServiceBase
{
    public UrsaDialogServce(IDialogManager dialogManager, Func<Type, object?>? viewModelFactory) : 
        base(dialogManager, viewModelFactory)
    {
        
    }
}