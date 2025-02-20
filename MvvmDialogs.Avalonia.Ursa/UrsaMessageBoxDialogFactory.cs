using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Ursa.Controls;
using MessageBoxButton = HanumanInstitute.MvvmDialogs.FrameworkDialogs.MessageBoxButton;

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
        DialogOptions options = new()
        {
            Title = settings.Title,
            Mode = ConvertDialogMode(settings.Icon),
            Button = ConvertDialogButton(settings.Button),
        };
        var window = new DefaultDialogWindow()
        {
            Content = new TextBlock() { Text = text }
        };
        ConfigureDialogWindow(window, options);
        var ownerRef = owner?.GetRef();
        switch (ownerRef)
        {
            case Window ownerWin:
            {
                var result = await window.ShowDialog<DialogResult>(ownerWin);
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
    
    private DialogMode ConvertDialogMode(MessageBoxImage icon) =>
        icon switch
        {
            MessageBoxImage.None => DialogMode.None,
            MessageBoxImage.Error => DialogMode.Error,
            MessageBoxImage.Information => DialogMode.Info,
            MessageBoxImage.Warning => DialogMode.Warning,
            _ => DialogMode.None
        };

    private DialogButton ConvertDialogButton(MessageBoxButton button) =>
        button switch
        {
            MessageBoxButton.Ok => DialogButton.OK,
            MessageBoxButton.YesNo => DialogButton.YesNo,
            MessageBoxButton.YesNoCancel => DialogButton.YesNoCancel,
            _ => DialogButton.OK
        };
    
    /// <summary>
    /// Attach options to dialog window.
    /// </summary>
    /// <param name="window"></param>
    /// <param name="options"></param>
    private static void ConfigureDialogWindow(DialogWindow window, DialogOptions? options)
    {
        options ??= new DialogOptions();
        window.WindowStartupLocation = options.StartupLocation;
        window.Title = options.Title;
        window.ShowInTaskbar = options.ShowInTaskBar;
        if (options.StartupLocation == WindowStartupLocation.Manual)
        {
            if (options.Position is not null)
            {
                window.Position = options.Position.Value;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }
    }
}