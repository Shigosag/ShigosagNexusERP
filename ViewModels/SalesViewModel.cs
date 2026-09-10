using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShigosagNexusERP.Models;
using ShigosagNexusERP.Services;

namespace ShigosagNexusERP.ViewModels;

public partial class SalesViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    private readonly INotificationService _notifier;

    public ObservableCollection<Order> Orders { get; } = new();

    [ObservableProperty]
    private decimal _newOrderAmount = 500.00m;

    [ObservableProperty]
    private string _newOrderStatus = "Pending";

    [ObservableProperty]
    private string _newOrderNotes = "Standard Client Purchase";

    public SalesViewModel(IDataService dataService, INotificationService notifier)
    {
        _dataService = dataService;
        _notifier = notifier;
        _ = LoadOrdersAsync();
    }

    [RelayCommand]
    public async Task LoadOrdersAsync()
    {
        try
        {
            IsBusy = true;
            Orders.Clear();
            var list = await _dataService.GetOrdersAsync();
            foreach (var o in list)
            {
                Orders.Add(o);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task CreateOrderAsync()
    {
        if (NewOrderAmount <= 0)
        {
            _notifier.Notify("Order amount must be greater than zero.", NotificationType.Warning);
            return;
        }

        var order = new Order
        {
            TotalAmount = NewOrderAmount,
            Status = NewOrderStatus,
            Notes = NewOrderNotes,
            CustomerId = 1
        };

        await _dataService.AddOrderAsync(order);
        _notifier.Notify($"Order recorded for {order.TotalAmount:C}", NotificationType.Success);
        await LoadOrdersAsync();
    }

    [RelayCommand]
    public async Task MarkCompletedAsync(Order? order)
    {
        if (order == null) return;
        await _dataService.UpdateOrderStatusAsync(order.Id, "Completed");
        order.Status = "Completed";
        _notifier.Notify($"Order #{order.Id} marked as Completed.", NotificationType.Success);
        await LoadOrdersAsync();
    }
}
