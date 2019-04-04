using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

//For using folders.
using myWebApp.Pages.Chat;
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
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
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=WebStoreDatabase.db");
            optionsBuilder.UseSqlite(@"Data Source=C:\Users\Sebas\Documents\GitHub\I4PRJ4\myWebApp\WebStoreDatabase.db");
        
        }
    }
}