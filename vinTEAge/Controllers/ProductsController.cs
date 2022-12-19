using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vinTEAge.Data;
using vinTEAge.Models;

namespace vinTEAge.Controllers
{
    //[Authorize]

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProductsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //afisarea tuturor produselor din baza de date, impreuna cu categoria din care fac parte:
        //HttpGet implicit
        //[Authorize(Roles = "User,Editor,Admin")]
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
        //[Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            Product product = db.Products.Include("Category").Include("Reviews").Where(prod => prod.ProductId == id).First();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(product);
        }

        //afisarea formularului in care se vor completa datele unui produs, impreuna cu categoria din care face parte:
        //HttpGet implicit
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult New()
        {
            Product product = new Product();

            product.Categ = GetAllCategories();

            return View(product);
        }

        //adaugarea produsului in baza de date:
        [Authorize(Roles = "Editor,Admin")]
        [HttpPost]
        public IActionResult New(Product product)
        {
            product.Reviews = null;
            
            product.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost adaugat";
                return RedirectToAction("Index");
            }
            else
            {
                product.Categ = GetAllCategories();
                return View(product);
            }
        }

        // se editeaza un produs existent in baza de date impreuna cu categoria din care face parte
        // categoria se selecteaza dintr-un dropdown
        // HttpGet implicit
        // se afiseaza formularul impreuna cu datele aferente articolului din baza de date
        [Authorize(Roles = "Editor,Admin")]
        public IActionResult Edit(int id)
        {
            Product product = db.Products.Include("Category").Where(prod => prod.ProductId == id).First();

            product.Categ = GetAllCategories();

            if (product.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(product);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine";
                return RedirectToAction("Index");
            }
        }

        //se adauga articolul modificat in baza de date
        [Authorize(Roles = "Editor,Admin")]
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
        [Authorize(Roles = "Editor,Admin")]
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
