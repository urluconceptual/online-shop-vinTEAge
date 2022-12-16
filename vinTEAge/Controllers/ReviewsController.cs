using Microsoft.AspNetCore.Mvc;

namespace vinTEAge.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
