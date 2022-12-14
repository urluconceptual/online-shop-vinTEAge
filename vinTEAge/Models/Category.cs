using System.ComponentModel.DataAnnotations;

namespace vinTEAge.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}
