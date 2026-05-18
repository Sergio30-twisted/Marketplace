using Catalogo.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Catalogoentidad = Catalogo.Domain.Entities.Catalogo;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultaCatalogoController : Controller
    {
        private readonly IConsultaDeCatalogoService _service;

        public ConsultaCatalogoController(IConsultaDeCatalogoService service)
        {
            _service = service;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Catalogoentidad>>> ObtenerTodosLosCatalogos()
        {
            var catalogos = await _service.ObtenerTodosLosCatalogos();
            return Ok(catalogos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Catalogoentidad>> ObtenerCatalogosPorId(int id)
        {
            var catalogo = await _service.ObtenerCatalogoPorId(id);

            if (catalogo == null)
            {
                return NotFound(new { mensaje = $"El catálogo con ID {id} no existe." });
            }

            return Ok(catalogo);
        }
    }
}
