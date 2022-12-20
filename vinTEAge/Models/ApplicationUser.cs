using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;


// PASUL 1 - useri si roluri 
namespace vinTEAge.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Review>? Reviews { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

        public virtual ICollection<InCart>? InCarts { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
