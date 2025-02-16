using System.ComponentModel;
using Ursa.Controls;

namespace MvvmDialogs.Avalonia.Ursa;

public class UrsaDialogSettings : DialogOptions
{
    public INotifyPropertyChanged? ViewModel { get; set; }
}