using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ShigosagNexusERP.Models;
using ShigosagNexusERP.Services;
using CommunityToolkit.Mvvm.Input;

namespace ShigosagNexusERP.ViewModels;

public partial class InventoryViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    public ObservableCollection<Product> Products { get; } = new();

    public InventoryViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var list = await _dataService.GetProductsAsync();
        Products.Clear();
        foreach (var item in list) Products.Add(item);
    }
}