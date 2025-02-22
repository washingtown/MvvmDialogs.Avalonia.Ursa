using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Microsoft.Extensions.DependencyInjection;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public partial class OverlayDialogViewModel : DialogContextViewModel, IModalDialogViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogService _overlayDialogService;
    private readonly IDialogService _windowDialogService;

    public OverlayDialogViewModel(
        IServiceProvider serviceProvider,
        [FromKeyedServices("Overlay")] IDialogService overlayDialogService,
        [FromKeyedServices("Window")] IDialogService windowDialogService,
        IDialogContextProvider dialogContextProvider
    ) : base(dialogContextProvider)
    {
        _serviceProvider = serviceProvider;
        _overlayDialogService = overlayDialogService;
        _windowDialogService = windowDialogService;
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
        var result = await _overlayDialogService.ShowMessageBoxAsync(
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
            _overlayDialogService.Show<SampleDialogViewModel>(this, vm);
            SampleViewResult = "";
        }
        else
        {
            var result = await _overlayDialogService.ShowDialogAsync<SampleDialogViewModel>(this, vm);
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
        var vm=scope.ServiceProvider.GetRequiredService<OverlayDialogViewModel>();
        if (!IsSampleViewModal)
        {
            _windowDialogService.Show(this, vm);
            SampleViewResult = "";
        }
        else
        {
            var result = await _windowDialogService.ShowDialogAsync(this, vm);
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