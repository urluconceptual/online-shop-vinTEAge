using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace vinTEAge.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu!")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu!")]
        public int Rating { get; set; }

        public virtual IdentityUser User { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public DateTime Date { get; set; }
    }
}
