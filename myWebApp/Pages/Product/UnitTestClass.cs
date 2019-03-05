//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;

using Xunit;

namespace myWebApp.Pages.Product
{
    public class UnitTestClass
    {
        [Fact]
        public void TestCreateProductId()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new CreateModel(dbContext);

            Product product = new Product();
            product.Id = 1;

            //Act
            dbContext.Products.Add(product);

            //Get product, which we just created.
            target.Product = dbContext.Products.FindAsync(1).Result;

            var actual = target.Product.Id;

            //Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public void TestCreateProductName()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new CreateModel(dbContext);

            Product product = new Product();
            product.Id = 1;
            product.Name = "TestName";

            //Act
            dbContext.Products.Add(product);

            //Get product, which we just created.
            target.Product = dbContext.Products.FindAsync(1).Result;

            var actual = target.Product.Name;

            //Assert
            Assert.Equal("TestName", actual);
        }

        [Fact]
        public void TestCreateProductDescription()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new CreateModel(dbContext);

            Product product = new Product();
            product.Id = 1;
            product.Description = "TestDescription";

            //Act
            dbContext.Products.Add(product);

            //Get product, which we just created.
            target.Product = dbContext.Products.FindAsync(1).Result;

            var actual = target.Product.Description;

            //Assert
            Assert.Equal("TestDescription", actual);
        }

        [Fact]
        public void TestCreateProductStock()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new CreateModel(dbContext);

            Product product = new Product();
            product.Id = 1;
            product.Stock = 1;

            //Act
            dbContext.Products.Add(product);

            //Get product, which we just created.
            target.Product = dbContext.Products.FindAsync(1).Result;

            var actual = target.Product.Stock;

            //Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public void TestCreateProductPrice()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new CreateModel(dbContext);

            Product product = new Product();
            product.Id = 1;
            product.Price = 1;

            //Act
            dbContext.Products.Add(product);

            //Get product, which we just created.
            target.Product = dbContext.Products.FindAsync(1).Result;

            var actual = target.Product.Price;

            //Assert
            Assert.Equal(1, actual);
        }
    }
}