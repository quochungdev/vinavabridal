using Microsoft.AspNetCore.Mvc;

namespace VinavaFashionProject.Api.Controllers
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}