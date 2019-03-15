//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;

using Xunit;
using Microsoft.EntityFrameworkCore;
using System;

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

            //Create product with ID = 1, so we can find it.
            product.Id = 1;

            //Act
            dbContext.Products.Add(product);

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

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

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Name = "TestName";

            //Act
            dbContext.Products.Add(product);

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

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

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Description = "TestDescription";

            //Act
            dbContext.Products.Add(product);

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

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

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Stock = 1;

            //Act
            dbContext.Products.Add(product);

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

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

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Price = 1;

            //Act
            dbContext.Products.Add(product);

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

            var actual = target.Product.Price;

            //Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public void TestEditProductName()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new EditModel(dbContext);

            Product product = new Product();

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Name = "TestName";

            //Act
            dbContext.Products.Add(product);

            //Edit Product name
            product.Name = "EditedName";

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

            var actual = target.Product.Name;

            //Assert
            Assert.Equal("EditedName", actual);
        }

        [Fact]
        public void TestEditProductDescription()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new EditModel(dbContext);

            Product product = new Product();

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Description = "TestDescription";

            //Act
            dbContext.Products.Add(product);

            //Edit Product name
            product.Description = "EditedDescription";

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

            var actual = target.Product.Description;

            //Assert
            Assert.Equal("EditedDescription", actual);
        }

        [Fact]
        public void TestEditProductStock()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new EditModel(dbContext);

            Product product = new Product();

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Stock = 12;

            //Act
            dbContext.Products.Add(product);

            //Edit Product name
            product.Stock = 3;

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

            var actual = target.Product.Stock;

            //Assert
            Assert.Equal(3, actual);
        }

        [Fact]
        public void TestEditProductPrice()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new EditModel(dbContext);

            Product product = new Product();

            //Create product with ID = 1, so we can find it.
            product.Id = 1;
            product.Price = 1000;

            //Act
            dbContext.Products.Add(product);

            //Edit Product name
            product.Price = 500;

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Get product, which we just created.
            target.Product = dbContext.Products.Find(1);

            var actual = target.Product.Price;

            //Assert
            Assert.Equal(500, actual);
        }
    
        [Fact]
        public void TestDeleteProduct()
        {
            //Arrange
            AppDbContext dbContext = new AppDbContext();

            var target = new IndexModel(dbContext);

            for (int i = 0; i < 3; i++)
            {
                //Create 3 product.
                Product product = new Product();
                product.Id = i;

                //Act
                dbContext.Products.Add(product);
            }

            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Loop through List of product
            foreach (var product in dbContext.Products)
            {
                //Delete product with ID 1(the first one we created)
                if(product.Id == 1)
                {
                    dbContext.Products.Remove(product);
                }
            }
            
            //Save Changes to database.
            dbContext.SaveChangesAsync();

            //Count all products in 
            int result = 0;
            foreach (var product in dbContext.Products)
            {
                result ++;
            }

            var actual = result;

            //Assert
            Assert.Equal(2, actual);
        }
    }
}