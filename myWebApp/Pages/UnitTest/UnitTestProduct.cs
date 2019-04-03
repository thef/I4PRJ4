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

        [Fact]
        public async Task TestEditProductAsync()
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

                await db.Products.AddAsync(expectedProduct);
                await db.SaveChangesAsync();

                // Act
                var actualProduct = db.Products.Find(1);

                actualProduct.Name = "TestEditedName";
                actualProduct.Description = "TestEditedDescription";
                actualProduct.Price = 9999;
                actualProduct.Stock = 99;

                //Check for state-changes (some or all values) and attached new values.
                db.Attach(expectedProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();

                // Assert
                Assert.Equal(
                    expectedProduct,
                    actualProduct
                );
            }
        }

        [Fact]
        public async Task TestDeleteProductAsync()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestDbContextOptions()))
            {
                // Arrange
                var product = new Product();

                product.Id = 1;
                product.Name = "TestName";
                product.Description = "TestDescription";
                product.Price = 1000;
                product.Stock = 10;

                await db.Products.AddAsync(product);
                await db.SaveChangesAsync();

                // Act
                var actualProduct = db.Products.Find(1);
                db.Products.Remove(actualProduct);
                
                await db.SaveChangesAsync();

                var list = await db.Products.ToListAsync();

                var listEmpty = false;

                if(list.Count == 0)
                {
                    listEmpty = true;

                } else {

                    listEmpty = false;
                }

                // Assert
                Assert.True(
                    listEmpty
                );
            }
        }
    }
}