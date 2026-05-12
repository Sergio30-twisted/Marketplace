using Catalogo.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministracionDeCatalogosController : Controller
    {
       
        private readonly IAdministracionDeCatalogosService _administracionService;


        // Inyectamos el servicio que ya tiene la conexión a la colección "Products"
        public AdministracionDeCatalogosController(IAdministracionDeCatalogosService administracionDeCatalogosService)
        {
            _administracionService = administracionDeCatalogosService;
        }

        // --- ACTUALIZAR (UPDATE) ---
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Catalogo.Domain.Entities.Catalogo catalogoActualizado)
        {
            // Validación de seguridad: el ID del objeto debe coincidir con el de la URL
            if (id != catalogoActualizado.Id)
            {
                return BadRequest(new { mensaje = "El ID del catálogo no coincide con el de la URL." });
            }

            var actualizado = await _administracionService.UpdateAsync(id, catalogoActualizado);

            if (actualizado)
            {
                return Ok(new { mensaje = $"Catálogo con ID {id} actualizado con éxito." });
            }

            return NotFound(new { mensaje = $"No se pudo actualizar. Es posible que el catálogo con ID {id} no exista." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Ejecutamos la eliminación en la base de datos
            var eliminado = await _administracionService.DeleteAsync(id);

            if (eliminado)
            {
                return Ok(new { mensaje = $"Catálogo con ID {id} eliminado correctamente." });
            }

            return NotFound(new { mensaje = $"No se encontró el catálogo con ID {id}." });
        }
    }
}
