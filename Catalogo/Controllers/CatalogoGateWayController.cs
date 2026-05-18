using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers // Asegúrate de que el namespace coincida con tu proyecto
{
    [ApiController]
    [Route("api/Catalogo")] // El frontend busca esta ruta en el puerto 7203
    public class CatalogoGatewayController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogoGatewayController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodo()
        {
            var cliente = _httpClientFactory.CreateClient();

            // Aquí mandamos la petición al puerto real del Catálogo (5110)
            // Usamos la ruta exacta que tiene tu ConsultaCatalogoController
            var respuesta = await cliente.GetAsync("http://localhost:5110/api/ConsultaCatalogo");

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                return Content(contenido, "application/json");
            }

            return StatusCode((int)respuesta.StatusCode, "Error al conectar con el microservicio de Catálogo");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var cliente = _httpClientFactory.CreateClient();
            var respuesta = await cliente.GetAsync($"http://localhost:5110/api/ConsultaCatalogo/{id}");

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                return Content(contenido, "application/json");
            }

            return StatusCode((int)respuesta.StatusCode, "Producto no encontrado");
        }
    }
}
