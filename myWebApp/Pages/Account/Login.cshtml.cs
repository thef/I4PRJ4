using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace myWebApp.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationDbUser> _signInManager;

        private readonly ILogger<LoginModel> _logger;
        
        public LoginModel(
            SignInManager<ApplicationDbUser> signInManager,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        //Get the page which the User come from. Store in returnUrl.
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            //Used to set redirect-option. This will redirect User back to the page which they come from, 
            //after some process is completed.
            returnUrl = returnUrl ?? Url.Content("~/");

            //Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //Store the Url, which User come from.
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                //Sign in User with it's information.
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    StatusMessage = $"User was logged in! with Email: {Input.Email}.";

                    //Write to log
                    _logger.LogInformation($"User: {Input.Email} logged in.");

                    //Using LocalRedirect to ensures that the "returnUrl" is a route actually on your site. For safe.
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    StatusMessage = $"User was lockout due to to many failed logins in! with Email: {Input.Email}.";

                    //Write to log
                    _logger.LogWarning($"User: {Input.Email} locked out.");

                    //Redirect User to lockout-page.
                    return RedirectToPage("./Lockout");

                } else {

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
