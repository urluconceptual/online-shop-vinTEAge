using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using vinTEAge.Data;
using vinTEAge.Models;

namespace vinTEAge.Controllers
{

    public class ProductsController : Controller
    {
        private IWebHostEnvironment _env;

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProductsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IWebHostEnvironment env
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
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

            if (TempData.ContainsKey("messageCart"))
            {
                ViewBag.MessageCart = TempData["messageCart"];
            }

            var search = "";

            // MOTOR DE CAUTARE

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                // Cautare in articol (Title si Content)

                List<int> productIds = db.Products.Include("Category").Where
                                        (
                                         at => at.Title.Contains(search)
                                         || at.Description.Contains(search)
                                         || at.Category.CategoryName.Contains(search)
                                        ).Select(a => a.ProductId).ToList();

                // Cautare in comentarii (Content)
                List<int> productIdsOfCommentsWithSearchString = db.Reviews
                                        .Where
                                        (
                                         c => c.Text.Contains(search)
                                        ).Select(c => (int)c.ProductId).ToList();

                // Se formeaza o singura lista formata din toate id-urile selectate anterior
                List<int> mergedIds = productIds.Union(productIdsOfCommentsWithSearchString).ToList();


                // Lista articolelor care contin cuvantul cautat
                // fie in articol -> Title si Content
                // fie in comentarii -> Content
                products = db.Products.Where(product => mergedIds.Contains(product.ProductId))
                                      .Include("Category")
                                      .Include("User")
                                      .OrderBy(a => a.Title);

            }

            ViewBag.SearchString = search;

            //AFISARE PAGINATA
            // Alegem sa afisam 6 produse pe pagina
            int _perPage = 6;

            // Fiind un numar variabil de articole, verificam de fiecare data utilizand 
            // metoda Count()

            int totalItems = products.Count();


            // Se preia pagina curenta din View-ul asociat
            // Numarul paginii este valoarea parametrului page din ruta
            // /Products/Index?page=valoare

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            // Pentru prima pagina offsetul o sa fie zero
            // Pentru pagina 2 o sa fie 3 
            // Asadar offsetul este egal cu numarul de articole care au fost deja afisate pe paginile anterioare
            var offset = 0;

            // Se calculeaza offsetul in functie de numarul paginii la care suntem
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            // Se preiau articolele corespunzatoare pentru fiecare pagina la care ne aflam 
            // in functie de offset
            var paginatedProducts = products.Skip(offset).Take(_perPage);


            // Preluam numarul ultimei pagini

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            // Trimitem articolele cu ajutorul unui ViewBag catre View-ul corespunzator
            ViewBag.Products = paginatedProducts;

            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/Products/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Products/Index/?page";
            }

            return View();
        }

        //afisarea unui singur produs, in functie de id:
        //HttpGet implicit
        //[Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Show(int id)
        {
            Product product = db.Products.Include("Category")
                                         .Include("Reviews")
                                         .Include("User")
                                         .Include("Reviews.User")
                                         .Where(prod => prod.ProductId == id).First();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            SetAccessRights();

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
        public async  Task<IActionResult> New(Product product, IFormFile ProductImage)
        {
            product.Reviews = null;
            product.UserId = _userManager.GetUserId(User);
            var sanitizer = new HtmlSanitizer();

            if (ProductImage.Length > 0)
            {
                // Generam calea de stocare a fisierului
                var storagePath = Path.Combine(
                _env.WebRootPath, // Luam calea folderului wwwroot
                "images", // Adaugam calea folderului images
                ProductImage.FileName // Numele fisierului
                );
                // General calea de afisare a fisierului care va fi stocata in  baza de date
                var databaseFileName = "/images/" + ProductImage.FileName;
                // Uploadam fisierul la calea de storage
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(fileStream);
                }
                product.Photo = databaseFileName;
            }


            if (ModelState.IsValid)
            {
                product.Description = sanitizer.Sanitize(product.Description);

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
        public async Task<IActionResult> Edit(int id, Product requestProduct, IFormFile? ProductImage = null)
        {
            Product product = db.Products.Find(id);
            requestProduct.Categ = GetAllCategories();
            var sanitizer = new HtmlSanitizer();

            if (ProductImage != null)
            {
                if (ProductImage.Length > 0)
                {
                    // Generam calea de stocare a fisierului
                    var storagePath = Path.Combine(
                    _env.WebRootPath, // Luam calea folderului wwwroot
                    "images", // Adaugam calea folderului images
                    ProductImage.FileName // Numele fisierului
                    );
                    // General calea de afisare a fisierului care va fi stocata in  baza de date
                    var databaseFileName = "/images/" + ProductImage.FileName;
                    // Uploadam fisierul la calea de storage
                    using (var fileStream = new FileStream(storagePath, FileMode.Create))
                    {
                        await ProductImage.CopyToAsync(fileStream);
                    }
                    product.Photo = databaseFileName;
                }
            }

            if (ModelState.IsValid)
            {
                if (product.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    requestProduct.Description = sanitizer.Sanitize(requestProduct.Description);

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
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine";
                    return RedirectToAction("Index");
                }
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
            Product product = db.Products.Include("Reviews").Where(prod => prod.ProductId == id).First();

            if (product.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un produs care nu va apartine!";
                return RedirectToAction("Index");
            }
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

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Editor"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.EsteUser = User.IsInRole("User");

            ViewBag.EsteEditor = User.IsInRole("Editor");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }
    }
}
