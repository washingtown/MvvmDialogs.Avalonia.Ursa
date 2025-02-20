using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public partial class WindowDialogViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    public WindowDialogViewModel(
        IDialogService dialogService
        )
    {
        _dialogService = dialogService;
    }
    
    [ObservableProperty] 
    private string _messageboxTitle = string.Empty;
    [ObservableProperty]
    private string _message = string.Empty;
    [RelayCommand]
    private async Task ShowMessageBoxAsync()
    {
        await _dialogService.ShowMessageBoxAsync(null, Message, MessageboxTitle);
    }
}