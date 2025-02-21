using System.ComponentModel;

namespace MvvmDialogs.Avalonia.Ursa;

/// <summary>
/// Indicate a View Model has a higher owner. Used by <see cref="UrsaWindowDialogManager"/> to find owner Window/Host
/// </summary>
public interface IIndicateToplevel : INotifyPropertyChanged
{
    /// <summary>
    /// ViewModel of the owner Toplevel.
    /// </summary>
    public INotifyPropertyChanged? ToplevelViewModel { get; set; }
}