using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

//using identity ApplicationUser in Account-folder.
using myWebApp.Pages.Account;

namespace myWebApp.Pages.Roles
{
    [Authorize(Roles="Admin")]
    public class ManageRoles : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRoles(
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

        public List<SelectListItem> listUser { get; set; }

        public List<SelectListItem> listRole { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "User")]
            public string User { get; set; }

            [Display(Name = "Role")]
            public string RoleFromlist { get; set; }

            [Display(Name = "Role")]
            public string Role { get; set; }
        }

        public IActionResult OnGet()
        {
            //Create list with all Users created
            List<SelectListItem> listUsers = new List<SelectListItem>();
            foreach (var user in _userManager.Users)
            {
                listUsers.Add(new SelectListItem() { Value = user.Email, Text = user.Email });
            };
            listUser = listUsers;

            //Create list with all Roles created
            List<SelectListItem> listRoles = new List<SelectListItem>();
            foreach (var role in _roleManager.Roles)
            {
                listRoles.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            }
            listRole = listRoles;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
        if (ModelState.IsValid)
        {
             //Check if listRole or Input.Role is empty, if so replace it's value to the other.
             //Assign Input text field if not null over listRole.
            if(Input.RoleFromlist == null)
            {
                Input.RoleFromlist = Input.Role;

            } else if(Input.Role == null) {

                Input.Role = Input.RoleFromlist;

            } else if(Input.RoleFromlist != null & Input.Role != null) {

                Input.RoleFromlist = Input.Role;
            }

            //Checks if role already exists.
            var resultAlreadyCreatedRole = await _roleManager.RoleExistsAsync(Input.Role);

            //IF ROLE DOES NOT ALREADY EXIST!.
            if(resultAlreadyCreatedRole == false)
            {
                //Create the new role
                var role = new IdentityRole();
                role.Name = Input.Role;
                await _roleManager.CreateAsync(role);

                //Get current user.
                var selectedUser = await _userManager.FindByEmailAsync(Input.User);

                //Check if current user already has that role.
                var resultAlreadyInRole = await _userManager.IsInRoleAsync(selectedUser,role.Name);
                if(!resultAlreadyInRole)
                {
                    //Assign role to seleced user.
                    await _userManager.AddToRoleAsync(selectedUser, Input.Role);

                    StatusMessage = $"Role was created and assigned to selected user!";

                } else {

                    StatusMessage = $"User: '{selectedUser.Email}' is already assigned to that role!";
                }

            //IF ROLE ALREADY DOES EXIST!.
            } else {
                
                //Get current user.
                var selectedUser = await _userManager.FindByEmailAsync(Input.User);

                //Check if current user already has that role.
                var resultAlreadyInRole = await _userManager.IsInRoleAsync(selectedUser, Input.Role);
                if(!resultAlreadyInRole)
                {
                    //Assign role to seleced user.
                    await _userManager.AddToRoleAsync(selectedUser, Input.Role);

                    StatusMessage = $"Role was created and assigned to selected user!";

                } else {
                    
                    StatusMessage = $"User: '{selectedUser.Email}' is already assigned to that role!";
                }
            }

        }
        return RedirectToPage();
        }
    }
}