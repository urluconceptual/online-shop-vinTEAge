using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vinTEAge.Data;
using vinTEAge.Models;

namespace vinTEAge.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductsController (ApplicationDbContext context)
        {
            db = context;
        }

        //afisarea tuturor produselor din baza de date, impreuna cu categoria din care fac parte:
        //HttpGet implicit
        public IActionResult Index()
        {
            var products = db.Products.Include("Category");

            ViewBag.Products = products;

            return View();
        }

        //afisarea unui singur produs, in functie de id:
        //HttpGet implicit
        public IActionResult Show(int id)
        {
            Product product = db.Products.Include("Category").Where(prod => prod.ProductId == id).First();

            ViewBag.Product = product;

            ViewBag.Category = product.Category;

            return View();
        }

    }
}
