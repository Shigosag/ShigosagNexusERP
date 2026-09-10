using System;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShigosagNexusERP.Data;
using ShigosagNexusERP.Services;
using ShigosagNexusERP.ViewModels;
using ShigosagNexusERP.Views;

namespace ShigosagNexusERP;

public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public App()
    {
        this.DispatcherUnhandledException += (s, e) =>
        {
            MessageBox.Show($"Activity Subsystem Fault: {e.Exception.Message}", 
                            "Nexus Runtime Diagnostics", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        };

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();

        var services = new ServiceCollection();

        // 1. Persist Configuration in DI
        services.AddSingleton<IConfiguration>(_configuration);

        // 2. Database Context Registration
        services.AddDbContext<AppDbContext>();

        // 3. Security, Tokenization, and Notifications
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddScoped<IDataService, DataService>();

        // 4. ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<InventoryViewModel>();
        services.AddTransient<SalesViewModel>();
        services.AddTransient<CRMViewModel>();
        services.AddTransient<HRViewModel>();
        services.AddTransient<FinanceViewModel>();
        services.AddTransient<SettingsViewModel>();

        // 5. Windows
        services.AddSingleton<MainWindow>(s => new MainWindow
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DbInitializer.Seed(context);

                // Authenticate default administrator and acquire JWT token
                var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
                await auth.LoginAsync("admin", "admin123");
            }

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Nexus Bootloader Exception: {ex.Message}\nTrace: {ex.InnerException?.Message}",
                            "Critical Boot Failure", MessageBoxButton.OK, MessageBoxImage.Hand);
            Shutdown();
        }
    }
}
