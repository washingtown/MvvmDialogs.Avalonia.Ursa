using Avalonia.Controls.Templates;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;
using MvvmDialogs.Avalonia.Ursa.Demo.Views;

namespace MvvmDialogs.Avalonia.Ursa.Demo;

public class UrsaViewLocator : StrongViewLocator
{
    public UrsaViewLocator()
    {
        Register<MainViewModel,MainWindow>();
        Register<WindowDialogViewModel, WindowDialogView>();
        Register<OverlayDialogViewModel, OverlayDialogView>();
    }
}