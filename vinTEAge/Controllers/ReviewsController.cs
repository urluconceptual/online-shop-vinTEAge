using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vinTEAge.Data;
using vinTEAge.Models; 

namespace vinTEAge.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReviewsController(ApplicationDbContext context)
        {
            db = context;
        }

        //adaugarea unui review asociat unui produs din baza de date 
        // HttpGet implicit
        // se afiseaza formularul in care se va intorduce review-ul impreuna cu ratingul
        public IActionResult New(int id)
        {
            Review review = new Review();
            review.ProductId = id; 
            return View(review); 
        }


        //adaugarea review-ului in baza de date:
        [HttpPost]
        public IActionResult New(int id, Review review)
        {
            if (ModelState.IsValid)
            {
                review.Date = DateTime.Now;
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
        public IActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);

            return View(review); 
        }

        //adaugarea review-ului in baza de date:
        [HttpPost]
        public IActionResult Edit(int id, Review requestReview)
        {
            Review review = db.Reviews.Find(id);

            if (ModelState.IsValid)
            {
                review.Text = requestReview.Text;
                review.Rating = requestReview.Rating;
                review.Date = DateTime.Now;
                TempData["message"] = "Review-ul a fost modificat!";
                db.SaveChanges();

                return Redirect("/Products/Show/" + review.ProductId);
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
        public IActionResult Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return Redirect("/Products/Show/" + review.ProductId);
        }

    }
}
