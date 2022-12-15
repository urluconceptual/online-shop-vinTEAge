using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vinTEAge.Data;
using vinTEAge.Models;

namespace vinTEAge.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductsController(ApplicationDbContext context)
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

        //afisarea formularului in care se vor completa datele unui produs, impreuna cu categoria din care face parte:
        //HttpGet implicit
        public IActionResult New()
        {
            var categories = from category in db.Categories
                             select category;

            ViewBag.Categories = categories;

            return View();
        }

        //adaugarea atributului in baza de date:
        [HttpPost]

        public IActionResult New(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("New");
            }
        }
    }
}
