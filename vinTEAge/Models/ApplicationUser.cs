using Microsoft.AspNetCore.Identity;


// PASUL 1 - useri si roluri 
namespace vinTEAge.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Review>? Reviews { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
