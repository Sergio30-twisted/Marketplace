using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Controllers
{
    public class Consulta_de_Catalogos : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
