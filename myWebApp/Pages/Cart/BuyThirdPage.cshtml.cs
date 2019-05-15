using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace myWebApp.Pages.Cart
{
    public class BuyThirdPageModel : PageModel
    {
        private readonly AppDbContext _db;
        private Random _randomizer = new Random();

        public BuyThirdPageModel(AppDbContext db)
        {
            _db = db;
        }

        public string Ordernum;

        public async Task<IActionResult> OnPostAsync()
        {
            var cartcontains = from crt in _db.Carts
                select crt;
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                cartcontains = cartcontains.Where(s => s.User.Email == User.Identity.Name);
                var carts = await cartcontains.ToListAsync();
                foreach (var cart in carts)
                {
                    _db.Carts.Remove(cart);
                }
                await _db.SaveChangesAsync();
            }

            var ordrnr = _randomizer.Next(99999999);
            Ordernum = ordrnr.ToString();
            if (Ordernum.Length < 8) {
                do
                {
                    Ordernum = '0' + Ordernum;
                } while (Ordernum.Length < 8);
            }
            return Page();

        }
    }
}