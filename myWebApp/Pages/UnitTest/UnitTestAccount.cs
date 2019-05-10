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
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
                    new Mock<FakeEmailSender>().Object
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

        [Theory]
        [InlineData("Test@Mail.com", "Qwerty1!")]
        public async Task TestLoginAccount(string email, string password)
        {   
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            {
                var signInManagerMock = new Mock<FakeSignInManager>();
                signInManagerMock.Setup(fsim => fsim.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                    .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

                // Arrange
                //---* FOR USAGE OF User.Identity *---//

                //Create test user
                var fakeIdentity = new GenericIdentity("Test@Mail.com");
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

                LoginModel loginModel = new LoginModel(
                    signInManagerMock.Object
                    )
                {
                    PageContext = pageContext
                };

            
                var inputModel = new LoginModel.InputModel();
                inputModel.Email = email;
                inputModel.Password = password;

                loginModel.Input = inputModel;

                // Act
                await loginModel.OnPostAsync("Index");

                var um = new FakeSignInManager();
                var loginResult = um.IsSignedIn(loginModel.User);

                // Assert
                Assert.True(
                    loginResult
                );
            }
        }

        /* Logout Test dosen't work, broken AF.
        [Fact]
        public async Task TestLogoutAccount()
        {   
            using (var db = new AppDbContext(Utilities.Utilities.TestAppDbContext()))
            {
                var signInManagerMock = new Mock<FakeSignInManager>(); 
                signInManagerMock.Setup(fsim => fsim.SignOutAsync())
                    .Returns(Task.FromResult(
                        new PageContext( new ActionContext( new DefaultHttpContext() 
                        { User = new ClaimsPrincipal( new GenericIdentity("notloggedin")) }, 
                            new RouteData(), 
                            new PageActionDescriptor(), 
                            new ModelStateDictionary()))));
                            
                // Arrange
                //---* FOR USAGE OF User.Identity *---//

                //Create test user
                var fakeIdentity = new GenericIdentity("Test@Mail.com");
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

                LogoutModel logoutModel = new LogoutModel(
                    signInManagerMock.Object
                    )
                {
                    PageContext = pageContext
                };

                // Act
                await logoutModel.OnPostAsync("Index");

                var um = new FakeSignInManager();
                var logoutResult = um.IsSignedIn(logoutModel.User);
                
                // Assert
                Assert.False(
                    logoutResult
                );
            }
        }
        */
    }
}