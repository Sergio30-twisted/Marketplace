using Microsoft.AspNetCore.Mvc;

namespace Auditoria_y_Trazabilidad.Controllers
{
    public class ReceptorDeLogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
