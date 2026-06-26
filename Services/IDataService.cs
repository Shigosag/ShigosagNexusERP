using System.Collections.Generic;
using System.Threading.Tasks;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public interface IDataService
{
    Task<List<Product>> GetProductsAsync();
    Task AddProductAsync(Product product);
}