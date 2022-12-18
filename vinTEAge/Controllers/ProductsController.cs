using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        //afisarea unui singur produs, in functie de id:
        //HttpGet implicit
        public IActionResult Show(int id)
        {
            Product product = db.Products.Include("Category").Include("Reviews").Where(prod => prod.ProductId == id).First();

            return View(product);
        }

        //afisarea formularului in care se vor completa datele unui produs, impreuna cu categoria din care face parte:
        //HttpGet implicit
        public IActionResult New()
        {
            Product product = new Product();

            product.Categ = GetAllCategories();

            return View(product);
        }

        //adaugarea atributului in baza de date:
        [HttpPost]

        public IActionResult New(Product product)
        {
            product.Reviews = null;
            product.Categ = GetAllCategories();

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost adaugat";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        // se editeaza un produs existent in baza de date impreuna cu categoria din care face parte
        // categoria se selecteaza dintr-un dropdown
        // HttpGet implicit
        // se afiseaza formularul impreuna cu datele aferente articolului din baza de date
        public IActionResult Edit(int id)
        {
            Product product = db.Products.Include("Category").Where(prod => prod.ProductId == id).First();

            product.Categ = GetAllCategories();

            return View(product);
        }

        //se adauga articolul modificat in baza de date
        [HttpPost]
        public IActionResult Edit(int id, Product requestProduct)
        {
            Product product = db.Products.Find(id);
            requestProduct.Categ = GetAllCategories();

            if (ModelState.IsValid)
            {
                product.Title = requestProduct.Title;
                product.Description = requestProduct.Description;
                product.Photo = requestProduct.Photo;
                product.CategoryId = requestProduct.CategoryId;
                product.Price = requestProduct.Price;
                TempData["message"] = "Produsul a fost modificat!";
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(requestProduct);
            }
        }

        // se sterge un produs din baza de date 
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            TempData["message"] = "Produsul a fost sters!";
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;

            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            /* Sau se poate implementa astfel: 
             * 
            foreach (var category in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = category.Id.ToString();
                listItem.Text = category.CategoryName.ToString();

                selectList.Add(listItem);
             }*/


            // returnam lista de categorii
            return selectList;
        }
    }
}
