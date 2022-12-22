using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vinTEAge.Models;

namespace vinTEAge.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<InCart> InCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<InCart>().HasKey(ab => new
            {
                ab.Id,
                ab.ProductId,
                ab.UserId
            }) ;
            // definire relatii cu modelele ApplicationUser si Product (FK)
            modelBuilder.Entity<InCart>()
            .HasOne(ab => ab.Product)
            .WithMany(ab => ab.InCarts)
            .HasForeignKey(ab => ab.ProductId);
            modelBuilder.Entity<InCart>()
            .HasOne(ab => ab.User)
            .WithMany(ab => ab.InCarts)
            .HasForeignKey(ab => ab.UserId);
        }
    }

}