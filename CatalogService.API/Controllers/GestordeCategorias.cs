using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
    public class GestordeCategorias : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
