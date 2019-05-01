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

namespace myWebApp.Pages.Product
{
    public class Startup
    {
        public Startup()
        {
            var users = new List<ApplicationDbUser>
            {
                new ApplicationDbUser
                {
                    UserName = "Test@Mail.com",
                    Id = Guid.NewGuid().ToString(),
                    Email = "Test@Mail.com"
                }
            }.AsQueryable();

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Customer",
                    Id = Guid.NewGuid().ToString()
                }
            }.AsQueryable();
         
            var fakeUserManager = new Mock<FakeUserManager>();
            fakeUserManager.Setup(fum => fum.Users)
                .Returns(users);

            fakeUserManager.Setup(fum => fum.CreateAsync(It.IsAny<ApplicationDbUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);


            var userValidator = new Mock<IUserValidator<ApplicationDbUser>>();
            userValidator.Setup(uv => uv.ValidateAsync(It.IsAny<UserManager<ApplicationDbUser>>(), It.IsAny<ApplicationDbUser>()))
                .ReturnsAsync(IdentityResult.Success);


            var passwordValidator = new Mock<IPasswordValidator<ApplicationDbUser>>();
            passwordValidator.Setup(ph => ph.ValidateAsync(It.IsAny<UserManager<ApplicationDbUser>>(), It.IsAny<ApplicationDbUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);


            var fakeSignInManager = new Mock<FakeSignInManager>();
            fakeSignInManager.Setup(fsim => fsim.PasswordSignInAsync(It.IsAny<ApplicationDbUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);


            var fakeRoleManager = new Mock<FakeRoleManager>();
            fakeRoleManager.Setup(frm => frm.Roles)
                .Returns(roles);

            fakeRoleManager.Setup(frm => frm.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
        }
    }

    public class UnitTestAccount : IClassFixture<Startup>
    {
        [Theory]
        [InlineData("Test@User.com", "Qwerty1!")]
        public async Task TestCreateAccount(string email, string password)
        {   
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            {
                // Arrange
                RegisterModel registerModel = new RegisterModel(
                    new Mock<FakeUserManager>().Object,
                    new Mock<FakeSignInManager>().Object,
                    new Mock<FakeRoleManager>().Object,
                    new Mock<FakeEmailSender>().Object,
                    new Mock<FakeLogger>().Object
                    );
            
                var indexModel = new RegisterModel.InputModel();

                indexModel.Email = email;
                indexModel.Password = password;

                registerModel.Input = indexModel;

                var user = new ApplicationDbUser
                {
                    UserName = email,
                    Email = email
                };

                // Act
                var createdUser = await registerModel.OnPostAsync();

                // BadRequestResult (400), NotFoundResult (404), and OkObjectResult (200)
                var result = createdUser.Equals(200);

                // Assert
                Assert.True(
                    result
                );
            }
        }
    }
}