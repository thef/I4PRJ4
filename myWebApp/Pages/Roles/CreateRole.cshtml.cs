using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

//using identity ApplicationUser in Account-folder.
using myWebApp.Pages.Account;

namespace myWebApp.Pages.Roles
{
    public class CreateRoleModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateRoleModel(
            UserManager<ApplicationDbUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [TempData]
        public String StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "User")]
            public string User { get; set; }

            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Rolename")]
            public string Role { get; set; }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
        if (ModelState.IsValid)
        {
            //Get user from userName(email)
            var user = await _userManager.GetUserAsync(User);

            //Checks if role already exists.
            var resultAlreadyCreatedRole = await _roleManager.RoleExistsAsync(Input.Role);

            //if it DOES NOT already exists.
            if(resultAlreadyCreatedRole == false)
            {
                StatusMessage = $"Role was created and assigned to selected user!";

                //Create the new role
                var role = new IdentityRole();
                role.Name = Input.Role;
                await _roleManager.CreateAsync(role);

            } else {
                
                StatusMessage = $"Role was assigned to selected user!";

                //Create role to seleced user form above.
                var result = await _userManager.AddToRoleAsync(user, Input.Role);
            }

        }
        return Page();
        }
    }
}