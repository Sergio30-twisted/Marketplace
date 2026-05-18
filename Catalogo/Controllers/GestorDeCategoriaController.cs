using Catalogo.Application.Interface;
using Catalogo.Model;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestorDeCategoriaController : Controller
    {
        private readonly IGestorDeCategoriaService _catalogoService;

        // Inyectamos el servicio que ya usas para los catálogos generales
        public GestorDeCategoriaController(IGestorDeCategoriaService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        [HttpGet("categoria/{nombreCategoria}")]
        public async Task<IActionResult> GetByCategoria(CategoriaCatalogo nombreCategoria)
        {
            // Supongamos que usas un servicio para filtrar en MongoDB
            // filter: x => x.Categoria == nombreCategoria
            var resultados = await _catalogoService.GetByCategoriaAsync(nombreCategoria);
            return Ok(resultados);
        }
    }
}
