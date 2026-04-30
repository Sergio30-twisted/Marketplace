using System.Net.Http.Json;

namespace OrderService.API.Services;

public class InventoryService
{
    private readonly HttpClient _http;

    public InventoryService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> ReservarProducto(int productId, int cantidad)
    {
        var url = $"https://localhost:5001/catalog/reservar?productId={productId}&cantidad={cantidad}";

        var response = await _http.PostAsync(url, null);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Error al reservar stock");
            return false;
        }

        Console.WriteLine("Stock reservado desde CatalogService");
        return true;
    }
}