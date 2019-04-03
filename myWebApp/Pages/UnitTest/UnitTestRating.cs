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
    public class UnitTestRating
    {
        [Fact]
        public async Task TestCreateRatingAsync()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestDbContextOptions()))
            {
                // Arrange
                Rating expectedRating = new Rating();
                expectedRating.Id = 1;
                expectedRating.ProductId = 1;
                expectedRating.Rate = 3;
                expectedRating.UserName = "TestUser";

                // Act
                await db.Rates.AddRangeAsync(expectedRating);
                await db.SaveChangesAsync();

                var ratingId = 1;
                var actualRating = db.Rates.Find(ratingId);

                // Assert
                Assert.Equal(
                    expectedRating,
                    actualRating
                );
            }
        }
        
        [Fact]
        public async Task TestOverallRating()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestDbContextOptions()))
            {
                // Arrange
                IndexModel uut = new IndexModel(db);

                Rating r = new Rating();
                r.Id = 1;
                r.ProductId = 1;
                r.Rate = 5;
                r.UserName = "Test";

                db.Rates.Add(r);

                /* 
                for (int i = 0; i < 5; i++)
                {
                    Rating expectedRating = new Rating();
                    expectedRating.Id = i;
                    expectedRating.ProductId = 1;
                    expectedRating.Rate = i;

                    db.Rates.Add(expectedRating);
                }
                */
                await db.SaveChangesAsync();

                // Act
                var ProductId = 1;
                var actualResult = uut.OverallRating(ProductId);

                var expectedResult = 5.0;

                // Assert
                Assert.Equal(
                    expectedResult,
                    actualResult
                );
            }
        }
    }
}