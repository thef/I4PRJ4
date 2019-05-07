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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;

//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using myWebApp.Pages.Utilities;


namespace myWebApp.Pages.Product
{
    public class UnitTestRating
    {
        [Fact]
        public async Task TestCreateRating()
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
                var actualOverallRating = uut.AverageRating(ProductId).Result;

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
            using (var fakeUserManager = new Mock<FakeUserManager>().Object)
            {
                //---* FOR USAGE OF User.Identity.Name *---//

                //Create test user
                var fakeIdentity = new GenericIdentity("User");
                var principal = new ClaimsPrincipal(fakeIdentity);
                var httpContext = new DefaultHttpContext()
                {
                    User = principal
                };

                //need these as well for the page context
                var modelState = new ModelStateDictionary();
                var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
                var modelMetadataProvider = new EmptyModelMetadataProvider();
                var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);

                // need page context for the page model
                var pageContext = new PageContext(actionContext) 
                {
                    ViewData = viewData
                };

                // Arrange
                var indexModel = new IndexModel(
                    db, 
                    fakeUserManager
                    )
                {
                    //Set PageContext
                    PageContext = pageContext
                };


                Product productToRate = new Product();
                productToRate.Id = 1;

                await db.Products.AddAsync(productToRate);

                // Act
                var productId = 1;
                var rating = 5;
                await indexModel.OnPostRateAsync(productId, rating);

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
        
        [Fact]
        public async Task TestUserHasRatedProduct()
        {
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            using (var fakeUserManager = new Mock<FakeUserManager>().Object)
            {
                //---* FOR USAGE OF User.Identity.Name *---//

                //Create test user
                var fakeIdentity = new GenericIdentity("User");
                var principal = new ClaimsPrincipal(fakeIdentity);
                var httpContext = new DefaultHttpContext()
                {
                    User = principal
                };

                //need these as well for the page context
                var modelState = new ModelStateDictionary();
                var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
                var modelMetadataProvider = new EmptyModelMetadataProvider();
                var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);

                // need page context for the page model
                var pageContext = new PageContext(actionContext) 
                {
                    ViewData = viewData
                };

                // Arrange
                var indexModel = new IndexModel(
                    db, 
                    fakeUserManager
                    )
                {
                    //Set PageContext
                    PageContext = pageContext
                };

                Rating rating = new Rating();
                rating.Id = 1;
                rating.ProductId = 1;
                rating.UserName = "User";

                await db.Rates.AddAsync(rating);
                await db.SaveChangesAsync();
                
                // Act
                var productId = 1;
                var expectedResult = await indexModel.UserHasRatedProduct(productId);

                // Assert
                Assert.True(
                    expectedResult
                );
            }
        }
    }
}