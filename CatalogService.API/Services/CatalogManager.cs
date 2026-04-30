using CatalogService.API.Models;

namespace CatalogService.API.Services;

public class CatalogManager
{
    private static List<Product> _products = new()
    {
        new Product { Id = 1, Nombre = "Manzanas", Precio = 20, Stock = 10 },
        new Product { Id = 2, Nombre = "Peras", Precio = 30, Stock = 5 }
    };

    public List<Product> GetAll()
    {
        return _products;
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public Product Add(Product product)
    {
        product.Id = _products.Count + 1;
        _products.Add(product);
        return product;
    }

    public bool UpdateStock(int productId, int cantidad)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);

        if (product == null || product.Stock < cantidad)
            return false;

        product.Stock -= cantidad;
        return true;
    }
}
