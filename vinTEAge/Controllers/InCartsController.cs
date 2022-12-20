using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vinTEAge.Data;
using vinTEAge.Models;

namespace vinTEAge.Controllers
{
    [Authorize]
    public class InCartsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InCartsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        //        [Authorize(Roles = "User,Editor,Admin")]
        //        // fiecare utilizator vede propriul cos de cumparaturi
        //        // HttpGet - implicit
        //        public IActionResult Index()
        //        {
        //            if (TempData.ContainsKey("message"))
        //            {
        //                ViewBag.Message = TempData["message"];
        //            }

        //            SetAccessRights();

        //            if (User.IsInRole("User") || User.IsInRole("Editor"))
        //            {
        //                var bookmarks = from bookmark in db.Bookmarks.Include("User")
        //                               .Where(b => b.UserId == _userManager.GetUserId(User))
        //                                select bookmark;

        //                ViewBag.Bookmarks = bookmarks;

        //                return View();
        //            }
        //            else
        //            if (User.IsInRole("Admin"))
        //            {
        //                var bookmarks = from bookmark in db.Bookmarks.Include("User")
        //                                select bookmark;

        //                ViewBag.Bookmarks = bookmarks;

        //                return View();
        //            }

        //            else
        //            {
        //                TempData["message"] = "Nu aveti drepturi";
        //                return RedirectToAction("Index", "Articles");
        //            }

        //        }

        // Afisarea tuturor articolelor pe care utilizatorul le-a salvat in 
        // bookmark-ul sau 

        [Authorize(Roles = "User")]
        public IActionResult Show()
        {

            if (User.IsInRole("User") )
            {
                var cartNotEmpty = db.InCarts
                                  .Include("Product")
                                  .Where(b => b.ProductId == )
                                  .Where(b => b.UserId == _userManager.GetUserId(User))
                                  .FirstOrDefault();

                if (cartNotEmpty == null)
                {
                    TempData["message"] = "Nu aveti drepturi";
                    return RedirectToAction("Index", "Articles");
                }

                return View(cartNotEmpty);
            }
            return View(); 
        }


        //        [Authorize(Roles = "User,Editor,Admin")]
        //        public IActionResult New()
        //        {
        //            return View();
        //        }

        //        [HttpPost]
        //        [Authorize(Roles = "User,Editor,Admin")]
        //        public ActionResult New(Bookmark bm)
        //        {
        //            bm.UserId = _userManager.GetUserId(User);

        //            if (ModelState.IsValid)
        //            {
        //                db.Bookmarks.Add(bm);
        //                db.SaveChanges();
        //                TempData["message"] = "Colectia a fost adaugata";
        //                return RedirectToAction("Index");
        //            }

        //            else
        //            {
        //                return View(bm);
        //            }
        //        }


        //        // Conditiile de afisare a butoanelor de editare si stergere
        //        private void SetAccessRights()
        //        {
        //            ViewBag.AfisareButoane = false;

        //            if (User.IsInRole("Editor") || User.IsInRole("User"))
        //            {
        //                ViewBag.AfisareButoane = true;
        //            }

        //            ViewBag.EsteAdmin = User.IsInRole("Admin");

        //            ViewBag.UserCurent = _userManager.GetUserId(User);
        //        }
    }
    }
