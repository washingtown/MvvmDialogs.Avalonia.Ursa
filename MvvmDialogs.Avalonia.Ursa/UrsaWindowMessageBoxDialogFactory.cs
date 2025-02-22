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

public class UrsaWindowMessageBoxDialogFactory : UrsaMessageBoxDialogFactoryBase
{
    public UrsaWindowMessageBoxDialogFactory(IDialogFactory? chain) : base(chain)
    {
    }

    protected override async Task<bool?> ShowMessageBoxDialogAsync(IView? owner, MessageBoxSettings settings)
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
                messageWindow.Icon ??= ownerWin.Icon;
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
}