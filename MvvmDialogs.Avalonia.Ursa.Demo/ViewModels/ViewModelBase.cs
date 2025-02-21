using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public class ViewModelBase : ObservableObject, IIndicateToplevel
{
    public virtual INotifyPropertyChanged? ToplevelViewModel { get; set; }
}