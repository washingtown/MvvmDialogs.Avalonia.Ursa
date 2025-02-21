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

public class UrsaWindowMessageBoxDialogFactory : DialogFactoryBase
{
    public UrsaWindowMessageBoxDialogFactory(IDialogFactory? chain) : base(chain)
    {
    }

    public override async Task<object?> ShowDialogAsync<TSettings>(IView? owner, TSettings settings)=>
        settings switch
        {
            MessageBoxSettings s => await ShowMessageBoxDialogAsync(owner, s).ConfigureAwait(true),
            _ => await base.ShowDialogAsync(owner, settings).ConfigureAwait(true)
        };

    private async Task<bool?> ShowMessageBoxDialogAsync(IView? owner, MessageBoxSettings settings)
    {
        string text = settings.Content;
        
        var messageWindow = new MessageBoxWindow(ToMessageBoxButton(settings.Button))
        {
            Content = settings.Content,
            Title = settings.Title,
            MessageIcon = ToMessageBoxIcon(settings.Icon),
        };
        var ownerRef = owner?.GetRef();
        switch (ownerRef)
        {
            case Window ownerWin:
            {
                var result = await messageWindow.ShowDialog<DialogResult>(ownerWin);
                return result switch
                {
                    DialogResult.Yes => true,
                    DialogResult.OK => true,
                    DialogResult.No => false,
                    DialogResult.Cancel => null,
                    _ => null
                };
            }
            default:
                throw new InvalidCastException("Owner must be of type Window.");
        }
    }
    private UrsaMessageBoxButton ToMessageBoxButton(MessageBoxButton button) =>
        button switch
        {
            MessageBoxButton.Ok => UrsaMessageBoxButton.OK,
            MessageBoxButton.YesNo => UrsaMessageBoxButton.YesNo,
            MessageBoxButton.OkCancel => UrsaMessageBoxButton.OKCancel,
            MessageBoxButton.YesNoCancel => UrsaMessageBoxButton.YesNoCancel,
            _ => UrsaMessageBoxButton.OK
        };
    
    private UrsaMessageBoxIcon ToMessageBoxIcon(MessageBoxImage icon) =>
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