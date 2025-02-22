using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs.Avalonia.Ursa.Demo.ViewModels;
using MvvmDialogs.Avalonia.Ursa.Demo.Views;

namespace MvvmDialogs.Avalonia.Ursa.Demo;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<UrsaViewLocator>()
            .AddScoped<IViewLocator>(provider => provider.GetRequiredService<UrsaViewLocator>());
        
        serviceCollection.AddScoped<IDialogContextProvider, DialogContextProvider>();
        
        serviceCollection.AddKeyedSingleton<IDialogService,UrsaWindowDialogService>("Window");
        serviceCollection.AddKeyedSingleton<IDialogService,UrsaOverlayDialogService>("Overlay");
        serviceCollection.AddSingleton<MainViewModel>()
            .AddTransient<WindowDialogViewModel>()
            .AddTransient<OverlayDialogViewModel>()
            .AddTransient<SampleDialogViewModel>()
            .AddTransient<SampleDialogWindowViewModel>();
        
        Services = serviceCollection.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            var viewLocator = Services.GetRequiredService<UrsaViewLocator>();
            DataTemplates.Add(viewLocator);
            var mainVM = Services.GetRequiredService<MainViewModel>();
            var mainWindow = (MainWindow)viewLocator.Create(mainVM);
            mainWindow.DataContext = mainVM;
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}