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
        // se afiseaza formularul impreuna cu datele aferente produsului din baza de date
        public IActionResult New(int id)
        {
            Product product = db.Products.Include("Reviews").Where(prod => prod.ProductId == id).First();

            ViewBag.Product = product;
            ViewBag.Reviews = product.Reviews;

            return View(); 
        }

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
       
    }
}
