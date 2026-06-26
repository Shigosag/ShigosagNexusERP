using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ShigosagNexusERP.Data;
using ShigosagNexusERP.Services;
using ShigosagNexusERP.ViewModels;
using ShigosagNexusERP.Views;

namespace ShigosagNexusERP;

/// <summary>
/// Interaction logic for App.xaml
/// Handles Dependency Injection, Startup Sequence, and Global Error Logging.
/// Author: Shigosag
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        // 1. GLOBAL UI EXCEPTION HANDLER
        // Catches unexpected UI crashes and provides a diagnostic message.
        this.DispatcherUnhandledException += (s, e) =>
        {
            MessageBox.Show($"System Activity Error: {e.Exception.Message}", 
                            "Shigosag Nexus - UI Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        };

        var services = new ServiceCollection();

        // 2. INFRASTRUCTURE & PERSISTENCE
        services.AddDbContext<AppDbContext>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IDataService, DataService>();

        // 3. VIEWMODEL REGISTRATION (All Modules)
        // Main Shell
        services.AddSingleton<MainViewModel>();
        
        // Transient views ensure data is fresh every time the user clicks the sidebar
        services.AddTransient<DashboardViewModel>(); 
        services.AddTransient<InventoryViewModel>();
        services.AddTransient<SalesViewModel>();
        services.AddTransient<CRMViewModel>();
        services.AddTransient<HRViewModel>();
        services.AddTransient<FinanceViewModel>();
        services.AddTransient<SettingsViewModel>();

        // 4. MAIN UI WINDOW
        services.AddSingleton<MainWindow>(s => new MainWindow()
        {
            DataContext = s.GetRequiredService<MainViewModel>()
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            // 5. NEXUS BOOTLOADER SEQUENCE
            // Pre-seeds the database and verifies infrastructure integrity
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                // Ensures the SQLite file and Enterprise tables exist
                context.Database.EnsureCreated();
                
                // Populates the corporate records (Employees, Orders, etc.)
                DbInitializer.Seed(context);
            }

            // 6. LAUNCH COMMAND CENTER
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            // Explicit error message if the database is locked or the boot sequence fails
            MessageBox.Show($"Nexus Bootloader Failure: {ex.Message}\n\nTrace: {ex.InnerException?.Message}", 
                            "Critical System Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            
            Shutdown();
        }
    }
}