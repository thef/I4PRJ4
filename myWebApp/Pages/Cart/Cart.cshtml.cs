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
        private readonly AppDbContext _db;

        public CartModel(AppDbContext db)
        {
            _db = db;
        }
        private readonly UserManager<ApplicationDbUser> _userManager;
        public CartModel(UserManager<ApplicationDbUser> userManager)
        {
            _userManager = userManager;
        }

        public int getprice(cart c)
        {
            var price = c.Quantity * c.Product.Price;
            return price;
        }

        public List<cart> cartlist { get; set; }

        public async Task OnGetAsync()
        {
            foreach (var cart in _db.Carts.ToList())
            {
                if (cart.User.Id == User.Identity.Name)
                {
                    cartlist.Add(cart);
                }
            }
        }
    }
}