using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using Microsoft.Extensions.DependencyInjection;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IViewLocator _viewLocator;
    private readonly IServiceProvider _serviceProvider;
    
    public MainViewModel(
        IServiceProvider serviceProvider,
        IViewLocator viewLocator,
        WindowDialogViewModel windowDialogViewModel)
    {
        _serviceProvider = serviceProvider;
        _viewLocator = viewLocator;
        
        SelectedViewModel = _serviceProvider.GetRequiredService<WindowDialogViewModel>();
    }

    [ObservableProperty] 
    private ViewModelBase _selectedViewModel;
    
    [RelayCommand]
    private void Navigate(string key)
    {
        Type? viewModelType = Type.GetType($"{GetType().Namespace}.{key}ViewModel");
        if(viewModelType == null)
            return;
        SelectedViewModel = (ViewModelBase)_serviceProvider.GetRequiredService(viewModelType);
    }
}