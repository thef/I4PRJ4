using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class FakeRoleManager : RoleManager<IdentityRole>
    {
        public FakeRoleManager() : base(
            new Mock<IRoleStore<IdentityRole>>().Object,
            new Mock<IEnumerable<IRoleValidator<IdentityRole>>>().Object,
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<IdentityRole>>>().Object
        )
        {
              
        }
    }
}