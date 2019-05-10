using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

//For using folders
using myWebApp.Pages.Account;

namespace myWebApp.Pages.Utilities
{
    public class FakeSignInManager : SignInManager<ApplicationDbUser>
    {
        public FakeSignInManager() : base(
            new Mock<FakeUserManager>().Object,
            new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<ApplicationDbUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<ApplicationDbUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object
        ) { }

        public override bool IsSignedIn(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Identity.Name == "Test@Mail.com";
        }
    }
}