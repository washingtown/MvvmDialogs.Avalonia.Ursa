using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ursa.Controls;
using MessageBoxButton = HanumanInstitute.MvvmDialogs.FrameworkDialogs.MessageBoxButton;
using UrsaMessageBoxButton = Ursa.Controls.MessageBoxButton;
using UrsaMessageBoxIcon = Ursa.Controls.MessageBoxIcon;

namespace MvvmDialogs.Avalonia.Ursa;

public abstract class UrsaMessageBoxDialogFactoryBase : DialogFactoryBase
{
    protected UrsaMessageBoxDialogFactoryBase(IDialogFactory? chain) : base(chain)
    {
    }
    
    public override async Task<object?> ShowDialogAsync<TSettings>(IView? owner, TSettings settings)=>
        settings switch
        {
            MessageBoxSettings s => await ShowMessageBoxDialogAsync(owner, s).ConfigureAwait(true),
            _ => await base.ShowDialogAsync(owner, settings).ConfigureAwait(true)
        };

    protected abstract Task<bool?> ShowMessageBoxDialogAsync(IView? owner, MessageBoxSettings settings);
    
    protected UrsaMessageBoxButton ToMessageBoxButton(MessageBoxButton button) =>
        button switch
        {
            MessageBoxButton.Ok => UrsaMessageBoxButton.OK,
            MessageBoxButton.YesNo => UrsaMessageBoxButton.YesNo,
            MessageBoxButton.OkCancel => UrsaMessageBoxButton.OKCancel,
            MessageBoxButton.YesNoCancel => UrsaMessageBoxButton.YesNoCancel,
            _ => UrsaMessageBoxButton.OK
        };
    
    protected UrsaMessageBoxIcon ToMessageBoxIcon(MessageBoxImage icon) =>
        icon switch
        {
            MessageBoxImage.None => UrsaMessageBoxIcon.None,
            MessageBoxImage.Error => UrsaMessageBoxIcon.Error,
            MessageBoxImage.Information => UrsaMessageBoxIcon.Information,
            MessageBoxImage.Warning => UrsaMessageBoxIcon.Warning,
            MessageBoxImage.Stop => UrsaMessageBoxIcon.Stop,
            _ => UrsaMessageBoxIcon.None
        };
}