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

        // Afisarea tuturor produselor pe care utilizatorul le-a salvat in 
        // cosul sau 

        [Authorize(Roles = "User")]
        public IActionResult Show()
        {
            var idUser = _userManager.GetUserId(User);
            var cart = db.Users.Where(x => x.Id == idUser).Include("InCarts").Include("InCarts.Product").FirstOrDefault();

            ViewBag.ProductsInCart = cart;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult New(InCart cart)
        {
            db.InCarts.Add(cart);
            db.SaveChanges();
            TempData["message"] = "Colectia a fost adaugata";
            return Redirect("/Products/Show/" + cart.ProductId);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Delete(InCart aux)
        {
            var cart = db.InCarts.Where(a => a.Id == aux.Id && a.ProductId == aux.ProductId && a.UserId == aux.UserId).First();
            db.InCarts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Show");
        }
    }
}
