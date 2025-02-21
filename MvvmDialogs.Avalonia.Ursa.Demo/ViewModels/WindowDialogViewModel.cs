using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Microsoft.Extensions.DependencyInjection;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public partial class WindowDialogViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogService _dialogService;
    public WindowDialogViewModel(
        IServiceProvider serviceProvider,
        IDialogService dialogService
        )
    {
        _serviceProvider = serviceProvider;
        _dialogService = dialogService;
    }
    
    [ObservableProperty] private string _messageboxTitle = "Title";
    [ObservableProperty] private string _message = "This is the message";
    [ObservableProperty] private MessageBoxImage _icon = MessageBoxImage.None;
    [ObservableProperty] private MessageBoxButton _buttons = MessageBoxButton.Ok;
    [ObservableProperty] private string _messageBoxResult = "null";
    
    [ObservableProperty] private bool _isSampleViewModal = true;
    [ObservableProperty] private string _sampleViewResult = "";
    
    [RelayCommand]
    private async Task ShowMessageBoxAsync()
    {
        var result = await _dialogService.ShowMessageBoxAsync(
            ownerViewModel: null,
            text: Message,
            title: MessageboxTitle,
            button: Buttons,
            icon: Icon
        );
        MessageBoxResult = result switch
        {
            true => "True",
            false => "False",
            _ => "Null"
        };
    }

    [RelayCommand]
    private async Task ShowSampleViewAsync()
    {
        var vm = _serviceProvider.GetRequiredService<SampleDialogViewModel>();
        if (!IsSampleViewModal)
        {
            _dialogService.Show<SampleDialogViewModel>(null, vm);
            SampleViewResult = "";
        }
        else
        {
            var result = await _dialogService.ShowDialogAsync<SampleDialogViewModel>(this, vm);
            SampleViewResult = result switch
            {
                true => "True",
                false => "False",
                _ => "Null"
            };
        }
    }
}