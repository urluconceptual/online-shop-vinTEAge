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
            Product product = db.Products.Include("Reviews").Where(prod => prod.ProductId == id).First();

            ViewBag.Product = product;

            ViewBag.Reviews = product.Reviews;

            return View(); 
        }


        //adaugarea review-ului in baza de date:
        [HttpPost]
        public IActionResult New(int id, Review review)
        {
            Product product = db.Products.Find(id);
 

            try
            {
                db.Reviews.Add(review); 
                db.SaveChanges();
                return Redirect("/Products/Show/" + id); 
            }
            catch (Exception)
            {
                return RedirectToAction("New", id); 
            }
        }

        //modificarea unui review asociat unui produs din baza de date 
        // HttpGet implicit
        // se afiseaza formularul in care se va intorduce review-ul impreuna cu ratingul
        public IActionResult Edit(int id)
        {
            Review review = db.Reviews.Find(id);
            ViewBag.Review = review;  

            return View(); 
        }

        //adaugarea review-ului in baza de date:
        [HttpPost]
        public IActionResult Edit(int id, Review requestReview)
        {
            Review review = db.Reviews.Find(id); 

            try
            {
                review.Text = requestReview.Text;
                review.Rating = requestReview.Rating;
                review.Date = requestReview.Date; 
                db.SaveChanges();

                return Redirect("/Products/Show/" + review.ProductId);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", id); 
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
