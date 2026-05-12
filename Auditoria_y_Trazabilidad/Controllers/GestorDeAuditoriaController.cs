using Microsoft.AspNetCore.Mvc;

namespace Auditoria_y_Trazabilidad.Controllers
{
    public class GestorDeAuditoriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
