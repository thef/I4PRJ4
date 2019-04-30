using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.SqlServer;

//For using folders.
using myWebApp.Pages.Chat;
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using myWebApp.Pages.Cart;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace myWebApp.Pages.Product
{
    public class AppDbContext : IdentityDbContext<ApplicationDbUser>
    { 
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Rating> Rates { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDeteails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Filename=WebStoreDatabase.db");
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433; Database=PRJ4; User Id=SA; Password=D15987532147er!");
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