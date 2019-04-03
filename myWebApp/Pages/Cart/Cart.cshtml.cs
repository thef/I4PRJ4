using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace myWebApp.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly UserManager<ApplicationDbUser> _userManager;
        public CartModel(UserManager<ApplicationDbUser> userManager)
        {
            _userManager = userManager;
        }

        public List<cart> Carts { get; set; }

        public async Task OnGetAsync()
        {
            var user = _userManager.FindByEmailAsync(User.Identity.Name);
            foreach (var cart in Carts)
            {
                
            }
        }
    }
}