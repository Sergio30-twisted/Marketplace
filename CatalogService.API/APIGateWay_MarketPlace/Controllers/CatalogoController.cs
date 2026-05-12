using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using APIGateWay_MarketPlace.Models;
using System.Net.Http;

namespace APIGateWay_MarketPlace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogoController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CatalogoController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    [HttpGet("categoria/{nombre}")]
    public async Task<IActionResult> GetCategoria(int id)
    {
        var client = _httpClientFactory.CreateClient();
        // Apuntamos al microservicio (7110) y al nuevo controlador que creaste
        var response = await client.GetAsync($"http://localhost:5110/api/GestorDeCategoria/categoria/{id}");

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "No se pudo obtener el filtro");

        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            using var client = new HttpClient();
            // IMPORTANTE: Asegúrate de que sea http (sin S) y el puerto 7110
            var response = await client.GetAsync("http://localhost:5110/api/ConsultaCatalogo");

            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                return Content(contenido, "application/json");
            }

            // Si falla, queremos ver qué error da el microservicio realmente
            var errorDetalle = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, $"Error del microservicio: {errorDetalle}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error de conexión: {ex.Message}");
        }
    }
    


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CatalogoDto catalogo)
    {
        // Configuración de la conexión a RabbitMQ
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // Declaramos la cola (debe coincidir con la de tu Backend Consumer)
        await channel.QueueDeclareAsync(queue: "queue.catalogo.crear",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

        // Convertimos el objeto a JSON

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var json = JsonSerializer.Serialize(catalogo);
        var body = Encoding.UTF8.GetBytes(json);

        // Publicamos el mensaje
        await channel.BasicPublishAsync(exchange: string.Empty,
                                 routingKey: "queue.catalogo.crear",
                                 body: body);

        return Accepted(new { mensaje = "Catálogo enviado al Event Bus" });
    }
}