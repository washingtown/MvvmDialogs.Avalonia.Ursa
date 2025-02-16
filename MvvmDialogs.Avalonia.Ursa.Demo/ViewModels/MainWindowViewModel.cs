using HanumanInstitute.MvvmDialogs;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    private readonly IViewLocator _viewLocator = new UrsaViewLocator();
}