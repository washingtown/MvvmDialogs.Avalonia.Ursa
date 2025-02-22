using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;
using MvvmDialogs.Avalonia.Ursa.Demo.Views;

namespace MvvmDialogs.Avalonia.Ursa.Demo;

public class UrsaViewLocator : StrongViewLocator
{
    private readonly IDialogContextProvider _dialogContextProvider;
    public UrsaViewLocator(IDialogContextProvider dialogContextProvider)
    {
        _dialogContextProvider = dialogContextProvider;
        
        RegisterViews();
    }

    public override object Create(object viewModel)
    {
        var control = base.Create(viewModel);
        if (control is Window window)
        {
            _dialogContextProvider.DialogContext = new DialogContext(window.GetHashCode(), null);
        }

        return control;
    }

    public void RegisterViews()
    {
        Register<MainViewModel,MainWindow>();
        Register<WindowDialogViewModel, WindowDialogView>();
        Register<OverlayDialogViewModel, OverlayDialogView>();
        Register<SampleDialogViewModel, SampleDialogView>();
        Register<SampleDialogWindowViewModel, SampleDialogWindow>();
    }
}