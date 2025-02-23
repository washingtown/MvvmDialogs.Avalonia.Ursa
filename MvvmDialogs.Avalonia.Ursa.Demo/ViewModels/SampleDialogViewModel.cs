using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using Ursa.Controls;

namespace MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;

public partial class SampleDialogViewModel : ViewModelBase, ICloseable, IModalDialogViewModel
{
    [ObservableProperty] private string? _city;
    [ObservableProperty] private string? _department;
    [ObservableProperty] private string? _owner;
    [ObservableProperty] private string? _target;
    public WindowNotificationManager? NotificationManager { get; set; }
    public WindowToastManager? ToastManager { get; set; }

    public SampleDialogViewModel()
    {
        Cities =
        [
            "Shanghai", "Beijing", "Hulunbuir", "Shenzhen", "Hangzhou", "Nanjing", "Chengdu", "Wuhan", "Chongqing",
            "Suzhou", "Tianjin", "Xi'an", "Qingdao", "Dalian"
        ];
        OKCommand = new RelayCommand(OK);
        CancelCommand = new RelayCommand(Cancel);
    }

    public ObservableCollection<string> Cities { get; set; }

    public void Close()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? RequestClose;

    public ICommand OKCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    
    private void OK()
    {
        DialogResult = true;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private void Cancel()
    {
        DialogResult = false;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }


    public bool? DialogResult { get; private set; }
}