using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShigosagNexusERP.Services;

namespace ShigosagNexusERP.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INotificationService _notifier;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private string _activeViewName = "Dashboard";

    [ObservableProperty]
    private string _toastMessage = string.Empty;

    [ObservableProperty]
    private bool _isToastVisible = false;

    [ObservableProperty]
    private string _toastColor = "#FF1493";

    [ObservableProperty]
    private string _authenticatedUser = "Administrator";

    public MainViewModel(IServiceProvider serviceProvider, INotificationService notifier, IAuthService authService)
    {
        _serviceProvider = serviceProvider;
        _notifier = notifier;
        _authService = authService;

        _notifier.NotificationTriggered += OnNotificationTriggered;
        AuthenticatedUser = _authService.CurrentUser?.Username ?? "Admin";

        NavigateToDashboard();
    }

    private void OnNotificationTriggered(string message, NotificationType type)
    {
        ToastMessage = message;
        ToastColor = type switch
        {
            NotificationType.Success => "#2ECC71",
            NotificationType.Warning => "#F1C40F",
            NotificationType.Error => "#E74C3C",
            _ => "#FF1493"
        };
        IsToastVisible = true;

        System.Windows.Threading.DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(3) };
        timer.Tick += (s, e) =>
        {
            IsToastVisible = false;
            timer.Stop();
        };
        timer.Start();
    }

    [RelayCommand]
    public void DismissToast()
    {
        IsToastVisible = false;
    }

    [RelayCommand]
    public void NavigateToDashboard()
    {
        CurrentView = _serviceProvider.GetRequiredService<DashboardViewModel>();
        ActiveViewName = "Dashboard";
    }

    [RelayCommand]
    public void NavigateToInventory()
    {
        CurrentView = _serviceProvider.GetRequiredService<InventoryViewModel>();
        ActiveViewName = "Inventory";
    }

    [RelayCommand]
    public void NavigateToSales()
    {
        CurrentView = _serviceProvider.GetRequiredService<SalesViewModel>();
        ActiveViewName = "Sales";
    }

    [RelayCommand]
    public void NavigateToCRM()
    {
        CurrentView = _serviceProvider.GetRequiredService<CRMViewModel>();
        ActiveViewName = "CRM";
    }

    [RelayCommand]
    public void NavigateToHR()
    {
        CurrentView = _serviceProvider.GetRequiredService<HRViewModel>();
        ActiveViewName = "HR";
    }

    [RelayCommand]
    public void NavigateToFinance()
    {
        CurrentView = _serviceProvider.GetRequiredService<FinanceViewModel>();
        ActiveViewName = "Finance";
    }

    [RelayCommand]
    public void NavigateToSettings()
    {
        CurrentView = _serviceProvider.GetRequiredService<SettingsViewModel>();
        ActiveViewName = "Settings";
    }
}
