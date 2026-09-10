using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShigosagNexusERP.Models;
using ShigosagNexusERP.Services;

namespace ShigosagNexusERP.ViewModels;

public partial class CRMViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    private readonly INotificationService _notifier;

    public ObservableCollection<Customer> Customers { get; } = new();

    [ObservableProperty]
    private string _newCustomerName = string.Empty;

    [ObservableProperty]
    private string _newCustomerEmail = string.Empty;

    [ObservableProperty]
    private string _newCustomerPhone = string.Empty;

    [ObservableProperty]
    private string _newCustomerCompany = string.Empty;

    public CRMViewModel(IDataService dataService, INotificationService notifier)
    {
        _dataService = dataService;
        _notifier = notifier;
        _ = LoadCustomersAsync();
    }

    [RelayCommand]
    public async Task LoadCustomersAsync()
    {
        try
        {
            IsBusy = true;
            Customers.Clear();
            var list = await _dataService.GetCustomersAsync();
            foreach (var c in list) Customers.Add(c);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task AddCustomerAsync()
    {
        if (string.IsNullOrWhiteSpace(NewCustomerName) || string.IsNullOrWhiteSpace(NewCustomerEmail))
        {
            _notifier.Notify("Customer Name and valid Email are required.", NotificationType.Warning);
            return;
        }

        var customer = new Customer
        {
            Name = NewCustomerName.Trim(),
            Email = NewCustomerEmail.Trim(),
            Phone = string.IsNullOrWhiteSpace(NewCustomerPhone) ? "N/A" : NewCustomerPhone.Trim(),
            Company = string.IsNullOrWhiteSpace(NewCustomerCompany) ? "Private Client" : NewCustomerCompany.Trim(),
            Status = "Active"
        };

        await _dataService.AddCustomerAsync(customer);
        _notifier.Notify($"Client {customer.Name} registered successfully.", NotificationType.Success);
        
        NewCustomerName = string.Empty;
        NewCustomerEmail = string.Empty;
        NewCustomerPhone = string.Empty;
        NewCustomerCompany = string.Empty;

        await LoadCustomersAsync();
    }
}
