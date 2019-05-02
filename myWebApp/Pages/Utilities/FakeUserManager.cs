using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
    public class FakeUserManager : UserManager<ApplicationDbUser>
    {
        public FakeUserManager() : base(
            new Mock<IUserStore<ApplicationDbUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationDbUser>>().Object,
            new IUserValidator<ApplicationDbUser>[0],
            //new List<IUserValidator<ApplicationDbUser>>(),
            new IPasswordValidator<ApplicationDbUser>[0],
            //new Mock<IEnumerable<IPasswordValidator<ApplicationDbUser>>>().Object,
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationDbUser>>>().Object
        ) { }

        public override Task<IdentityResult> CreateAsync(ApplicationDbUser user)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public override Task<ApplicationDbUser> FindByEmailAsync(string email)
        {
            return Task.FromResult( new ApplicationDbUser { Email = email } );
        }

        public override Task<bool> IsEmailConfirmedAsync(ApplicationDbUser user)
        {
            return Task.FromResult(user.Email == "Test@Mail.com");
        }

        //TODO FIX THIS
        public override Task<string> GeneratePasswordResetTokenAsync(ApplicationDbUser user)
        {
            return Task.FromResult("");
        }
    }
}