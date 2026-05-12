using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
    public class AdministraciondeCatalogo : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
