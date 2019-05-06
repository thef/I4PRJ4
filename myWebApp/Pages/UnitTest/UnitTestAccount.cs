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
    /*
    public class Startup
    {
        public Startup()
        {
            var users = new List<ApplicationDbUser>
            {
                new ApplicationDbUser
                {
                    UserName = "Test@Mail.com",
                    //Id = Guid.NewGuid().ToString(),
                    Email = "Test@Mail.com",
                }
            }.AsQueryable();

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Customer",
                    //Id = Guid.NewGuid().ToString()
                }
            }.AsQueryable();
         
            fakeUserManager = new Mock<FakeUserManager>();
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
    */


    public class UnitTestAccount //: IClassFixture<Startup>
    {
        [Theory]
        [InlineData("Test@Mail.com", "Qwerty1!")]
        public async Task TestRegisterAccount(string email, string password)
        {   
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            {
                var userManagerMock = new Mock<FakeUserManager>();
                userManagerMock.Setup(umm => umm.CreateAsync(It.IsAny<ApplicationDbUser>(), It.IsAny<string>()))
                    .ReturnsAsync(IdentityResult.Success);

                // Arrange
                RegisterModel registerModel = new RegisterModel(
                    userManagerMock.Object,
                    new Mock<FakeSignInManager>().Object,
                    new Mock<FakeRoleManager>().Object,
                    new Mock<FakeEmailSender>().Object,
                    new Mock<FakeLogger>().Object
                    );
            
                var inputModel = new RegisterModel.InputModel();
                inputModel.Email = email;
                inputModel.Password = password;

                registerModel.Input = inputModel;

                // Act
                await registerModel.OnPostAsync("Index");

                var um = new FakeUserManager();
                var createdUser = await um.FindByEmailAsync("Test@Mail.com");

                // Assert
                Assert.Equal(
                    "Test@Mail.com",
                    createdUser.Email
                );
            }
        }
    }
}