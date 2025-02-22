using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Microsoft.Extensions.DependencyInjection;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public partial class WindowDialogViewModel : DialogContextViewModel, IModalDialogViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogService _dialogService;

    public WindowDialogViewModel(
        IServiceProvider serviceProvider,
        [FromKeyedServices("Window")] IDialogService dialogService,
        IDialogContextProvider dialogContextProvider
    ) : base(dialogContextProvider)
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
            ownerViewModel: this,
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
            _dialogService.Show<SampleDialogViewModel>(this, vm);
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

    [RelayCommand]
    private async Task ShowSampleWindowAsync()
    {
        var vm = _serviceProvider.GetRequiredService<SampleDialogWindowViewModel>();
        if (!IsSampleViewModal)
        {
            _dialogService.Show<SampleDialogWindowViewModel>(this, vm);
            SampleViewResult = "";
        }
        else
        {
            var result = await _dialogService.ShowDialogAsync<SampleDialogWindowViewModel>(this, vm);
            SampleViewResult = result switch
            {
                true => "True",
                false => "False",
                _ => "Null"
            };
        }
    }
    
    [RelayCommand]
    private async Task ShowInNewWindowAsync()
    {
        var scope = _serviceProvider.CreateScope();
        var vm=scope.ServiceProvider.GetRequiredService<WindowDialogViewModel>();
        if (!IsSampleViewModal)
        {
            _dialogService.Show(this, vm);
            SampleViewResult = "";
        }
        else
        {
            var result = await _dialogService.ShowDialogAsync(this, vm);
            SampleViewResult = result switch
            {
                true => "True",
                false => "False",
                _ => "Null"
            };
        }
    }

    public bool? DialogResult { get; }
}