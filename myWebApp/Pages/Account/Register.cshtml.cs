using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myWebApp.Pages.Product;

namespace myWebApp.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationDbUser> userManager,
            SignInManager<ApplicationDbUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
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
            [Compare ("Password", ErrorMessage = "The password and confirm password do not match!")] 
            public string ConfirmPassword { get; set; }
        }

        //Get the page which the User come from. Store in returnUrl.
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //Used to set redirect-option. This will redirect User back to the page which they come from, 
            //after some process is completed.
            returnUrl = returnUrl ?? Url.Content("~/");
        
            if(ModelState.IsValid)
            {
                //Create new User
                var user = new ApplicationDbUser { UserName = Input.Email, Email = Input.Email };
                var userCreatedResult = await _userManager.CreateAsync(user, Input.Password);
                
                if(userCreatedResult.Succeeded)
                {
                    //Create first user as Admin.
                    if(_userManager.Users.ToList().Count == 1)
                    {
                        await FirstAccountAdmin(user);

                        //Using LocalRedirect to ensures that the "returnUrl" is a route actually on your site. For safe.
                        return LocalRedirect("/Index");

                    } else {

                        //Checks if role already exists.
                        var resultAlreadyCreatedRole = await _roleManager.RoleExistsAsync("Customer");

                        //If role DOES NOT already exists.
                        if(resultAlreadyCreatedRole == false)
                        {
                            //Create the new role.
                            var role = new IdentityRole();
                            role.Name = "Customer";
                            await _roleManager.CreateAsync(role);

                            //Assign role to seleced user.
                            await _userManager.AddToRoleAsync(user, role.Name);

                            StatusMessage = $"User created with Email: {Input.Email}. Please check your Email to confirm it.";

                            //Write to log
                            _logger.LogInformation($"User {Input.Email} was created.");

                        } else {

                        //Assign role to seleced user.
                        await _userManager.AddToRoleAsync(user, "Customer");

                        StatusMessage = $"User created with Email: {Input.Email}. Please check your Email to confirm it.";

                        //Write to log
                        _logger.LogInformation($"User {Input.Email} was assigned to role: Customer.");

                        }

                        //Sign created User in.
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        //Send emailconfirmation to new created user.
                        //var callbaclUrl = await GenerateEmailConfirmation(user);
                        //await SendEmailConfirmation(user, callbaclUrl);

                    //Using LocalRedirect to ensures that the "returnUrl" is a route actually on your site. For safe.
                    return LocalRedirect("/Index");
                    }
                }
                //Handle errors
                foreach (var error in userCreatedResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            //if we reach this, something failed! Just stay on current page.
            return Page();
        }

        //Use method to make first User Admin.
        public async Task FirstAccountAdmin(ApplicationDbUser user)
        {
            //Check role(Admin) is created
            if(!await _roleManager.RoleExistsAsync("Admin"))
            {
                //Create the new role: Admin
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Assign role to seleced user.
                await _userManager.AddToRoleAsync(user, role.Name);

                //Sign User in.
                await _signInManager.SignInAsync(user, isPersistent: false);

                //Send emailconfirmation to new created user.
                //var callbaclUrl = await GenerateEmailConfirmation(user);
            }
        }

        //GenerateEmailConfirmation for selected User.
        public async Task<string> GenerateEmailConfirmation(ApplicationDbUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code = code },
                protocol: Request.Scheme
            );

            return callbackUrl;
        }

        //SendEmailConfirmation for selected User.
        public async Task SendEmailConfirmation(ApplicationDbUser user, string callbackUrl)
        {
            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            );
        }
    }
}