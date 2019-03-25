using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using myWebApp.Pages.Utilities;
using System.Linq;

namespace myWebApp.Pages.Product
{
    public class UnitTestProduct
    {
        [Fact]
        public async Task TestCreateProductAsync()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestDbContextOptions()))
            {
                // Arrange
                var expectedProduct = new Product();

                expectedProduct.Id = 1;
                expectedProduct.Name = "TestName";
                expectedProduct.Description = "TestDescription";
                expectedProduct.Price = 1000;
                expectedProduct.Stock = 10;

                // Act
                await db.Products.AddAsync(expectedProduct);
                await db.SaveChangesAsync();

                var productId = 1;
                var actualProduct = db.Products.Find(productId);

                // Assert
                Assert.Equal(
                    expectedProduct,
                    actualProduct
                );
            }
        }
    }
}