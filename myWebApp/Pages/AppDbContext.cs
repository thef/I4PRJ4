using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

//For using folders.
using myWebApp.Pages.Chat;
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using myWebApp.Pages.Cart;
using Microsoft.Extensions.DependencyInjection;

namespace myWebApp.Pages.Product
{
    public class AppDbContext : DbContext
    { 
        public AppDbContext() : base() {}

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Rating> Rates { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<cart> Carts { get; set; }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=WebStoreDatabase.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart.cart>().HasKey(c => new { c.UserId, c.ProductId });
            modelBuilder.Entity<Cart.cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId);
            modelBuilder.Entity<Cart.cart>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(c => c.ProductId);
        }
    }
}