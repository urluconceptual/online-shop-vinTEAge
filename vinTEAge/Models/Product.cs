using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vinTEAge.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Numele produsului este obligatoriu!")]
        [StringLength(50, ErrorMessage = "Titlul nu poate avea mai mult de 50 de caractere!")]
        [MinLength(5, ErrorMessage = "Titlul trebuie sa aiba mai mult de 5 caractere!")]

        public string Title { get; set; }

        [Required(ErrorMessage = "Descrierea produsului este obligatorie!")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Poza cu produsul este obligatorie!")]
        public string? Photo { get; set; }

        [Required(ErrorMessage = "Pretul produsului este obligatoriu!")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Categoria produsului este obligatorie!")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public float? Rating { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }

        public virtual ICollection<InCart>? InCarts{ get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
    }
}
