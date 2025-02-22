using System.ComponentModel;

namespace MvvmDialogs.Avalonia.Ursa;

/// <summary>
/// ViewModel which owns a DialogContext. Used by <see cref="UrsaWindowDialogManager"/> to find owner Window/Host
/// </summary>
public interface IDialogContextOwner : INotifyPropertyChanged
{
    public DialogContext? DialogContext { get; set; }
}