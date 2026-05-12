using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace APIGateWay_MarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministracionDeCatalogosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdministracionDeCatalogosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"http://localhost:5110/api/AdministracionDeCatalogos/{id}");

            if (response.IsSuccessStatusCode)
                return Ok(new { mensaje = "Eliminado correctamente desde la Gateway" });

            return BadRequest(new { mensaje = "Error al eliminar en el microservicio" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JsonElement catalogoActualizado)
        {
            var client = _httpClientFactory.CreateClient();

            // Convertimos el objeto recibido a JSON string para enviarlo al microservicio
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(catalogoActualizado),
                Encoding.UTF8,
                "application/json");

            // Llamada al microservicio de catálogo en el puerto 7110
            var response = await client.PutAsync($"http://localhost:5110/api/AdministracionDeCatalogos/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { mensaje = "Catálogo actualizado correctamente desde la Gateway" });
            }

            return BadRequest(new { mensaje = "No se pudo actualizar el catálogo en el microservicio" });
        }

    }
}
