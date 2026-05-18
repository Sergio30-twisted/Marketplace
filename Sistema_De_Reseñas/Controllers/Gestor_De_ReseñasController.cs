using Microsoft.AspNetCore.Mvc;

namespace Sistema_De_Reseñas.Controllers
{
    public class Gestor_De_ReseñasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
