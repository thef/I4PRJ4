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
using myWebApp.Pages.Product;

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

        //List all users in dropdownlist view.
        public List<SelectListItem> listUser { get; set; }

        //List all roles in dropdownlist view.
        public List<SelectListItem> listRole { get; set; }

        //List of users in string format.
        public IList<string> Users { get; set; }

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

        public void OnGet()
        {
            listsForDropDowns();
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

                    StatusMessage = $"Role '{Input.Role}' was created and assigned to selected user '{Input.User}'";

                } else {

                    StatusMessage = $"Error: User '{selectedUser.Email}' is already assigned to role '{Input.Role}'";
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

                    StatusMessage = $"Role '{Input.Role}' was assigned to user '{Input.User}'.";

                } else {
                    
                    StatusMessage = $"Error: User '{selectedUser.Email}' is already assigned to role '{Input.Role}'";
                }
            }

        }
        return RedirectToPage();
        }

        public void listsForDropDowns()
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

            //Create list of User.
            List<string> listStringUsers = new List<string>();
            foreach (var user in _userManager.Users)
            {
                listStringUsers.Add(user.ToString());
            }
            Users = listStringUsers;
        }

        //Get roles for selected user by it's username.
        public IList<string> GetRolesForUser(string userName)
        {
            IList<string> roles = new List<string>();

            //Get all users and loop though them to find selected user.
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                //Check for selected userName
                if(user.UserName == userName)
                {
                    //Get roles for selected user.
                    roles =  _userManager.GetRolesAsync(user).Result;
                }
            }

            return roles;
        }

        //Deletehandler to delete user roles form users.
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            //Check if listRole or Input.Role is empty, if so replace it's value to the other.
            if(Input.RoleFromlist == null)
            {
                Input.RoleFromlist = Input.Role;

            } else if(Input.Role == null) {

                Input.Role = Input.RoleFromlist;
            }

            //Check user exist
            if(Input.User != null)
            {
                //Get all users and loop though them to find selected user.
                var users = _userManager.Users.ToList();

                foreach (var user in users)
                {
                    //Check for selected userName
                    if(user.UserName == Input.User)
                    {
                        //Check for selected userName is the the selected role.
                        var result = await _userManager.IsInRoleAsync(user, Input.Role);

                        //IF TRUE user is in the that role
                        if(result)
                        {
                            //Remove role from selected user.
                            await _userManager.RemoveFromRoleAsync(user, Input.Role);

                            StatusMessage = $"Role '{Input.Role}' was removed form '{user}'.";

                        } else {

                            StatusMessage = $"Error: User '{user}' is not in '{Input.Role}'.";
                        }
                    }
                }
            }

            return RedirectToPage();
        }        
        
    }
}