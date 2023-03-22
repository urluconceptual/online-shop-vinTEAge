using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vinTEAge.Data;
using vinTEAge.Models; 

namespace vinTEAge.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        //adaugarea unui review asociat unui produs din baza de date 
        // HttpGet implicit
        // se afiseaza formularul in care se va intorduce review-ul impreuna cu ratingul
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult New(int id)
        {
            Review review = new Review();
            review.ProductId = id; 
            return View(review); 
        }


        //adaugarea review-ului in baza de date:
        [Authorize(Roles = "User,Editor,Admin")]
        [HttpPost]
        public IActionResult New(int id, Review review)
        {
            if (ModelState.IsValid)
            {
                var product = db.Products.Find(review.ProductId);
                int nrReviews = db.Reviews.Where(p => p.ProductId == review.ProductId).Count();

                if (nrReviews != 0)
                {
                    product.Rating = ((int)(((product.Rating * nrReviews) + review.Rating) / (nrReviews + 1) * 10))/(float)10;
                }
                else
                {
                    product.Rating = review.Rating;
                }

                review.Date = DateTime.Now;
                review.UserId = _userManager.GetUserId(User);
                db.Reviews.Add(review); 
                db.SaveChanges();
                TempData["message"] = "Review-ul a fost adaugat cu succes!";
                return Redirect("/Products/Show/" + id); 
            }
            else
            {
                return View(review); 
            }
        }

        //modificarea unui review asociat unui produs din baza de date 
        // HttpGet implicit
        // se afiseaza formularul in care se va intorduce review-ul impreuna cu ratingul
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);

            if (review.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
            return View(review); 
        }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa modificati comentariul!";
                return Redirect("/Products/Show/" + review.ProductId);
            }

        }

        //adaugarea review-ului in baza de date:
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Edit(int id, Review requestReview)
        {
            Review review = db.Reviews.Find(id);

            if (ModelState.IsValid)
            {
                if (review.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    var product = db.Products.Find(review.ProductId);
                    int nrReviews = db.Reviews.Where(p => p.ProductId == review.ProductId).Count();

                    product.Rating = ((product.Rating * nrReviews) + review.Rating) / (nrReviews + 1);

                review.Text = requestReview.Text;
                review.Rating = requestReview.Rating;
                review.Date = DateTime.Now;
                TempData["message"] = "Review-ul a fost modificat!";
                db.SaveChanges();

                return Redirect("/Products/Show/" + review.ProductId);
            }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa modificati comentariul!";
                    return RedirectToAction("/Products/Show/" + review.ProductId);
                }

              
            }
            else
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                ViewBag.Errors = errors; 
                return View(requestReview); 
            }
        }

        //stergerea unui review asociat unui produs din baza de date
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);

            if (review.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
            db.Reviews.Remove(review);
            db.SaveChanges();
            return Redirect("/Products/Show/" + review.ProductId);
        }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti comentariul!";
                return RedirectToAction("/Products/Show/" + review.ProductId);
            }

            
        }

    }
}
