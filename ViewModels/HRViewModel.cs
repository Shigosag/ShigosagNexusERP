using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShigosagNexusERP.Models;
using ShigosagNexusERP.Services;

namespace ShigosagNexusERP.ViewModels;

public partial class HRViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    private readonly INotificationService _notifier;

    public ObservableCollection<Employee> Employees { get; } = new();

    [ObservableProperty]
    private string _newEmpName = string.Empty;

    [ObservableProperty]
    private string _newEmpDept = "IT";

    [ObservableProperty]
    private string _newEmpPosition = "Associate Engineer";

    public HRViewModel(IDataService dataService, INotificationService notifier)
    {
        _dataService = dataService;
        _notifier = notifier;
        _ = LoadEmployeesAsync();
    }

    [RelayCommand]
    public async Task LoadEmployeesAsync()
    {
        try
        {
            IsBusy = true;
            Employees.Clear();
            var list = await _dataService.GetEmployeesAsync();
            foreach (var e in list) Employees.Add(e);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task AddEmployeeAsync()
    {
        if (string.IsNullOrWhiteSpace(NewEmpName))
        {
            _notifier.Notify("Employee name is required.", NotificationType.Warning);
            return;
        }

        var emp = new Employee
        {
            Name = NewEmpName.Trim(),
            Department = NewEmpDept,
            Position = NewEmpPosition,
            Status = "Active"
        };

        await _dataService.AddEmployeeAsync(emp);
        _notifier.Notify($"Employee {emp.Name} onboarded.", NotificationType.Success);
        NewEmpName = string.Empty;
        await LoadEmployeesAsync();
    }

    [RelayCommand]
    public async Task ToggleStatusAsync(Employee? employee)
    {
        if (employee == null) return;
        var nextStatus = employee.Status == "Active" ? "On Leave" : "Active";
        await _dataService.UpdateEmployeeStatusAsync(employee.Id, nextStatus);
        employee.Status = nextStatus;
        _notifier.Notify($"{employee.Name} status updated to {nextStatus}.", NotificationType.Information);
        await LoadEmployeesAsync();
    }
}
