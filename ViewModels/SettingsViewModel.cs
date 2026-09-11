using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ShigosagNexusERP.Data;
using ShigosagNexusERP.Services;

namespace ShigosagNexusERP.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly AppDbContext _db;
    private readonly IAuthService _authService;
    private readonly INotificationService _notifier;
    private readonly IConfiguration _config;

    [ObservableProperty]
    private string _systemVersion = "v2.0.0 - Production Certified (PostgreSQL/JWT)";

    [ObservableProperty]
    private string _databaseStatus = "Checking...";

    [ObservableProperty]
    private string _currentOperator = "Authenticated System Admin";

    [ObservableProperty]
    private string _activeTokenPreview = "No token active";

    public SettingsViewModel(AppDbContext db, IAuthService authService, INotificationService notifier, IConfiguration config)
    {
        _db = db;
        _authService = authService;
        _notifier = notifier;
        _config = config;

        VerifyStatus();
    }

    [RelayCommand]
    public void VerifyStatus()
    {
        var provider = _config["DatabaseProvider"] ?? "SQLite";
        bool canConnect = _db.Database.CanConnect();
        
        DatabaseStatus = canConnect 
            ? $"Online ({provider} Enterprise)" 
            : $"Degraded / Offline ({provider})";

        CurrentOperator = _authService.CurrentUser?.FullName ?? "Administrator";
        ActiveTokenPreview = _authService.CurrentToken != null 
            ? $"{_authService.CurrentToken[..Math.Min(24, _authService.CurrentToken.Length)]}..." 
            : "No active token.";

        _notifier.Notify("Configuration status refreshed.", NotificationType.Information);
    }
}
