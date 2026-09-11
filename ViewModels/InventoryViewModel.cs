using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShigosagNexusERP.Models;
using ShigosagNexusERP.Services;

namespace ShigosagNexusERP.ViewModels;

public partial class InventoryViewModel : ViewModelBase
{
    private readonly IDataService _dataService;
    private readonly INotificationService _notifier;

    public ObservableCollection<Product> Products { get; } = new();
    private List<Product> _masterProductList = new();

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "All";

    // Form inputs for quick product registration
    [ObservableProperty]
    private string _newProductName = string.Empty;

    [ObservableProperty]
    private string _newProductSku = string.Empty;

    [ObservableProperty]
    private decimal _newProductPrice = 99.00m;

    [ObservableProperty]
    private int _newProductStock = 10;

    [ObservableProperty]
    private string _newProductCategory = "Hardware";

    [ObservableProperty]
    private Product? _selectedProduct;

    public InventoryViewModel(IDataService dataService, INotificationService notifier)
    {
        _dataService = dataService;
        _notifier = notifier;
        _ = LoadDataAsync();
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            IsBusy = true;
            var list = await _dataService.GetProductsAsync();
            _masterProductList = list;
            ApplyFilter();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void FilterByCategory(string category)
    {
        SelectedCategory = category;
        ApplyFilter();
    }

    partial void OnSearchQueryChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Products.Clear();
        var query = _masterProductList.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            query = query.Where(p => p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) 
                                  || p.SKU.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
        }

        if (SelectedCategory != "All" && !string.IsNullOrEmpty(SelectedCategory))
        {
            query = query.Where(p => p.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var item in query)
        {
            Products.Add(item);
        }
    }

    [RelayCommand]
    public async Task AddProductAsync()
    {
        if (string.IsNullOrWhiteSpace(NewProductName) || string.IsNullOrWhiteSpace(NewProductSku))
        {
            _notifier.Notify("Product Name and SKU are strictly required.", NotificationType.Warning);
            return;
        }

        var newProduct = new Product
        {
            Name = NewProductName.Trim(),
            SKU = NewProductSku.Trim().ToUpper(),
            Price = NewProductPrice,
            StockLevel = NewProductStock,
            Category = NewProductCategory
        };

        await _dataService.AddProductAsync(newProduct);
        _notifier.Notify($"Product {newProduct.SKU} added successfully.", NotificationType.Success);

        NewProductName = string.Empty;
        NewProductSku = string.Empty;
        await LoadDataAsync();
    }

    [RelayCommand]
    public async Task IncrementStockAsync(Product? product)
    {
        if (product == null) return;
        product.StockLevel += 1;
        await _dataService.UpdateProductAsync(product);
        _notifier.Notify($"Stock updated for {product.SKU}.", NotificationType.Information);
        ApplyFilter();
    }

    [RelayCommand]
    public async Task DeleteProductAsync(Product? product)
    {
        if (product == null) return;
        await _dataService.DeleteProductAsync(product.Id);
        _notifier.Notify($"Archived SKU: {product.SKU}", NotificationType.Warning);
        await LoadDataAsync();
    }
}
