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
using System.Data.SqlClient;

namespace myWebApp.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationDbUser> userManager,
            SignInManager<ApplicationDbUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        [TempData]
        public String StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        
        public string Username { get; set; }

        public class InputModel
        {
            [Required]
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be atlest {2} and max {1} charaters long!", MinimumLength = 6)] 
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare ("Password", ErrorMessage = "The password and confirm password no dot match!")] 
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
        
            if(ModelState.IsValid)
            {
                var user = new ApplicationDbUser { UserName = Input.Email, Email = Input.Email };
                var userCreatedResult = await _userManager.CreateAsync(user, Input.Password);
                
                if(userCreatedResult.Succeeded)
                {
                    //Checks if role already exists.
                     var resultAlreadyCreatedRole = await _roleManager.RoleExistsAsync("Customer");

                    //if it DOES NOT already exists.
                    if(resultAlreadyCreatedRole == false)
                    {
                        //Create the new role.
                        var role = new IdentityRole();
                        role.Name = "Customer";
                        await _roleManager.CreateAsync(role);

                        //Assign role to seleced user.
                        await _userManager.AddToRoleAsync(user, role.Name);

                        StatusMessage = $"User created with Email: {Input.Email}. Please check your Email to confirm it.";

                    } else {

                    //Assign role to seleced user.
                    await _userManager.AddToRoleAsync(user, "Customer");

                    StatusMessage = $"User created with Email: {Input.Email}. Please check your Email to confirm it.";

                    }

                    //Send email confirmation to new created user.
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme
                    );

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                         "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                    );

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                //Handle errors
                foreach (var error in userCreatedResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //if we reach this, something failed! Just redirect.
            return Page();
        }  
    }
}
