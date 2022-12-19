using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace vinTEAge.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Continutul review-ului este obligatoriu!")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Ratingul este obligatoriu!")]
        [Range(1, 5, ErrorMessage = "Ratingul trebuie sa fie un numar natural intre 1 si 5!")]
        public int Rating { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product? { get; set; }

        public DateTime Date { get; set; }
    }
}
