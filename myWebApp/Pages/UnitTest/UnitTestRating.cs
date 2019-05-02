using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Moq;

//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using myWebApp.Pages.Utilities;
using Microsoft.AspNetCore.Http;

namespace myWebApp.Pages.Product
{
    public class UnitTestRating
    {
        [Fact]
        public async Task TestCreateRatingAsync()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            {
                // Arrange
                Rating expectedRating = new Rating();
                expectedRating.Id = 1;
                expectedRating.ProductId = 1;
                expectedRating.Rate = 3;
                expectedRating.UserName = "TestUser";

                // Act
                db.Rates.AddRange(expectedRating);
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
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            using (var fakeUserManager = new Mock<FakeUserManager>().Object)
            {
                // Arrange
                IndexModel uut = new IndexModel(db, fakeUserManager);

                Rating expectedRating = new Rating();
                expectedRating.Id = 1;
                expectedRating.ProductId = 5;
                expectedRating.Rate = 5;

                Rating expectedRating2 = new Rating();
                expectedRating2.Id = 2;
                expectedRating2.ProductId = 5;
                expectedRating2.Rate = 3;

                Rating expectedRating3 = new Rating();
                expectedRating3.Id = 3;
                expectedRating3.ProductId = 5;
                expectedRating3.Rate = 1;

                await db.Rates.AddRangeAsync(expectedRating, expectedRating2, expectedRating3);
                await db.SaveChangesAsync();

                // Act
                var ProductId = 5;
                var actualOverallRating = uut.OverallRating(ProductId).Result;

                var expectedOverallRating = 3.0;

                // Assert
                Assert.Equal(
                    expectedOverallRating,
                    actualOverallRating
                );
            }
        }

        [Fact]
        public async Task TestRatingsDeletedOnDeleteProduct()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            using (var fakeUserManager = new Mock<FakeUserManager>().Object)
            {
                // Arrange
                IndexModel uut = new IndexModel(db, fakeUserManager);

                Rating expectedRating = new Rating();
                expectedRating.Id = 1;
                expectedRating.ProductId = 3;
                expectedRating.Rate = 5;

                Rating expectedRating2 = new Rating();
                expectedRating2.Id = 2;
                expectedRating2.ProductId = 3;
                expectedRating2.Rate = 3;

                Rating expectedRating3 = new Rating();
                expectedRating3.Id = 3;
                expectedRating3.ProductId = 3;
                expectedRating3.Rate = 1;

                Product productWithRatings = new Product();
                productWithRatings.Id = 3;

                await db.Rates.AddRangeAsync(expectedRating, expectedRating2, expectedRating3);
                await db.Products.AddAsync(productWithRatings);
                await db.SaveChangesAsync();

                // Act
                var ProductId = 3;
                await uut.OnPostDeleteAsync(ProductId);

                var listRatings = await db.Rates.AsNoTracking().ToListAsync();

                var actualRatingCount = listRatings.Count;

                var expectedRatingCount = 0;

                // Assert
                Assert.Equal(
                    expectedRatingCount,
                    actualRatingCount
                );
            }
        }

        [Fact]
        public async Task TestCreateRatingForProduct()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            using (var userManager = new Mock<FakeUserManager>().Object)
            {
                // Arrange
                IndexModel uut = new IndexModel(db, userManager);

                Product productToRate = new Product();
                productToRate.Id = 1;

                await db.Products.AddAsync(productToRate);

                // Act
                var productId = 1;
                var rating = 5;


                /*
                //Generate Identity for usage of User.Identity.Name
                var fakeIdentity = new GenericIdentity("TestUser");
                var fakeUserRole = new string[1]{"Costumer"};
                var fakePrincipal = new GenericPrincipal(fakeIdentity, fakeUserRole);
                */

                /* 
                //Create fake User.
                var user = new ApplicationDbUser
                {
                    UserName = "Test@User.com",
                    //Id = Guid.NewGuid().ToString(),
                    Email = "Test@User.com"
                };

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("name", user.UserName),
                };
                */


                var username = "Test@User.com";
                var identity = new GenericIdentity(username, "");

                var mockPrincipal = new Mock<ClaimsPrincipal>();
                mockPrincipal.Setup(x => x.Identity).Returns(identity);
                mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

                var mockHttpContext = new Mock<HttpContext>();
                mockHttpContext.Setup(m => m.User).Returns(mockPrincipal.Object);
                

                await uut.OnPostRateAsync(productId, rating);

                var productRating = db.Rates.Single(r => r.ProductId.Equals(productId));

                var actualProductRating = productRating.Rate;

                var expectedProductRating = 5.0;

                // Assert
                Assert.Equal(
                    actualProductRating,
                    expectedProductRating
                );
            }
        }

        
        [Fact]
        public async Task TestProductRatedYet()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            using (var fakeUserManager = new Mock<FakeUserManager>().Object)
            {
                // Arrange
                IndexModel uut = new IndexModel(db, fakeUserManager);

                Rating rating = new Rating();
                rating.Id = 1;
                rating.ProductId = 1;

                await db.Rates.AddAsync(rating);
                await db.SaveChangesAsync();

                // Act
                var productId = 1;
                var actualResult = await uut.RatedYet(productId);

                // Assert
                Assert.True(
                    actualResult
                );
            }
        }

        [Fact]
        public async Task TestProductNumberOfVotes()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            using (var fakeUserManager = new Mock<FakeUserManager>().Object)
            {
                // Arrange
                IndexModel uut = new IndexModel(db, fakeUserManager);

                Rating rating = new Rating();
                rating.Id = 1;
                rating.ProductId = 1;

                Rating rating2 = new Rating();
                rating2.Id = 2;
                rating2.ProductId = 1;

                await db.Rates.AddRangeAsync(rating, rating2);
                await db.SaveChangesAsync();

                // Act
                var productId = 1;
                var actualNumberOfVotes = await uut.NumberOfVotes(productId);

                var expectedNumberOfVotes = 2;

                // Assert
                Assert.Equal(
                    expectedNumberOfVotes,
                    actualNumberOfVotes
                );
            }
        }
        
        /*
        [Fact]
        public async Task TestUserHasRatedProduct()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            {
            // Arrange
            var userManager = new Mock<FakeUserManager>().Object;

            IndexModel uut = new IndexModel(db, userManager);

            Rating rating = new Rating();
            rating.Id = 1;
            rating.ProductId = 1;
            rating.UserName = "Test@User.com";

            await db.Rates.AddAsync(rating);
            await db.SaveChangesAsync();

            //Create fake User.
            var user = new ApplicationDbUser
            {
                UserName = "Test@User.com",
                Id = Guid.NewGuid().ToString(),
                Email = "Test@User.com"
            };

            var users = new List<ApplicationDbUser>
            {
                new ApplicationDbUser
                {
                    UserName = "Test@Mail.com",
                    Id = Guid.NewGuid().ToString(),
                    Email = "Test@Mail.com"
                }
            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();
            fakeUserManager.Setup(fum => fum.Users)
                .Returns(users);

            fakeUserManager.Setup(fum => fum.CreateAsync(It.IsAny<ApplicationDbUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var fakeSignInManager = new Mock<FakeSignInManager>();
            fakeSignInManager.Setup(fsim => fsim.PasswordSignInAsync(It.IsAny<ApplicationDbUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            var mock = new Mock<ClaimsPrincipal>();
            mock.SetupGet(cp => cp.Identity.Name).Returns(user.UserName);
            
            // Act
            var productId = 1;
            var expectedResult = await uut.UserHasRatedProduct(productId);

            // Assert
                Assert.True(
                    expectedResult
                );
            }
        }
        */
    }
}