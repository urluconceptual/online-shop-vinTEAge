using System.ComponentModel.DataAnnotations;

namespace vinTEAge.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Numele produsului este obligatoriu!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Descrierea produsului este obligatoriu!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Poza cu produsul este obligatorie!")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Pretul produsului este obligatoriu!")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Categoria produsului este obligatorie!")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public float? Rating { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
