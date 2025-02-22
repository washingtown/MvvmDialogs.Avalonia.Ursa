using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using HanumanInstitute.MvvmDialogs;

namespace MvvmDialogs.Avalonia.Ursa;

public static class DialogFactoryExtensions
{
    /// <summary>
    /// Registers Ursa Window MessageBox handlers in the dialog factory chain.
    /// </summary>
    /// <param name="factory">The dialog factory to add handlers for.</param>
    /// <returns>A new dialog factory that will fallback to the previous one.</returns>
    public static IDialogFactory AddUrsaWindowMessageBox(this IDialogFactory factory)
    {
        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime)
        {
            throw new NotSupportedException("UrsaWindowMessageBox is only supported in Desktop applications.");
        }
        return new UrsaWindowMessageBoxDialogFactory(factory);
    }
    
    /// <summary>
    /// Registers Ursa Overlay MessageBox handlers in the dialog factory chain.
    /// </summary>
    /// <param name="factory">The dialog factory to add handlers for.</param>
    /// <returns>A new dialog factory that will fallback to the previous one.</returns>
    public static IDialogFactory AddUrsaOverlayMessageBox(this IDialogFactory factory)
    {
        return new UrsaOverlayMessageBoxDialogFactory(factory);
    }
}