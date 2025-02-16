using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ursa.Controls;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaWindowDialogFactory : DialogFactoryBase
{
    public UrsaWindowDialogFactory(IDialogFactory? chain) : base(chain)
    {
    }

    public override async Task<object?> ShowDialogAsync<TSettings>(IView? owner, TSettings settings)
    {
        return settings switch
        {
            MessageBoxSettings ms=> await ShowUrsaWindowMessageBoxAsync(owner, ms),
            UrsaDialogSettings us => await ShowUrsaWindowDialogAsync(owner, us),
            _ => base.ShowDialogAsync(owner, settings)
        };
    }

    private async Task<bool?> ShowUrsaWindowMessageBoxAsync(IView? owner, MessageBoxSettings? settings)
    {
        
    }
    
    private async Task<bool?> ShowUrsaWindowDialogAsync(IView? owner, UrsaDialogSettings settings)
    {
    }
}