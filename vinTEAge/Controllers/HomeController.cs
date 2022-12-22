using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using vinTEAge.Data;
using vinTEAge.Models;

namespace vinTEAge.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<HomeController> logger

            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;

            _logger = logger;

        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Products");
            }

            var products = from product in db.Products
                           select product;

            ViewBag.FirstProduct = products.First();
            ViewBag.products = products.OrderBy(o => o.Rating).Skip(1).Take(2);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}