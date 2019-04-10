﻿using System;
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

        public CartModel(AppDbContext db, UserManager<ApplicationDbUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        private readonly UserManager<ApplicationDbUser> _userManager;

        public int getprice(cart c)
        {
            var price = c.Quantity * c.Product.Price;
            return price;
        }

        public int FullPrice()
        {
            int Price=0;
            foreach (var product in Carts)
            {
                Price += product.Product.Price;
            }

            return Price;
        }
        [TempData]
        public string StatusMessage { get; set; }

        public List<cart> Carts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var cartcontains = from crt in _db.Carts
                select crt;
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                cartcontains = cartcontains.Where(s => s.UserId==User.Identity.Name);
                //Check if we found anyting
                if (cartcontains.AsNoTracking().ToList().Count != 0)
                {
                    //Succes found a match
                }
                else
                {
                    //Failed no match reload page - Show all.
                    return RedirectToPage();
                }
            }
            else
            {
                //Do nothing if no search-string id entered.
            }
            //Load list of StudentGroups
            Carts = await cartcontains.AsNoTracking().ToListAsync();
            //Update current page.
            return Page();
        }

        public async Task<IActionResult> OnPostIncreaseQuantityAsync(int id)
        {
            var Product = await _db.Products.FindAsync(id);
            var Cart = await _db.Carts.FindAsync(new{User.Identity.Name,id});

            if (Product != null)
            {
                if (Product.Stock != 0)
                {
                    Product.Stock--;
                    _db.Attach(Product).State = EntityState.Modified;
                    Cart.Quantity++;
                    _db.Attach(Cart).State = EntityState.Modified;

                    await _db.SaveChangesAsync();
                    StatusMessage = $"Cart quantity of product with ID: {id} increased by one";
                }
                else
                {
                    StatusMessage = $"Error: Product with id: {id} out of stock!";
                }

            }
            else
            {

                StatusMessage = $"Error: Can't find product with ID: {id}!";
            }
            //Update current page.
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDecreaseQuantityAsync(int id)
        {
            var Product = await _db.Products.FindAsync(id);
            var Cart = await _db.Carts.FindAsync(new { User.Identity.Name, id });

            if (Product != null)
            {
                if (Cart.Quantity >1)
                {
                    Product.Stock++;
                    _db.Attach(Product).State = EntityState.Modified;
                    Cart.Quantity--;
                    _db.Attach(Cart).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    StatusMessage = $"Cart quantity of product with ID: {id} decreased by one";
                }
                else
                {
                    _db.Carts.Remove(Cart);
                    Product.Stock += Cart.Quantity;
                    _db.Attach(Product).State = EntityState.Modified;

                    await _db.SaveChangesAsync();

                    StatusMessage = $"Product with ID: {id} removed from cart";
                }

            }
            else
            {

                StatusMessage = $"Error: Can't find product with ID: {id}!";
            }
            //Update current page.
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(int id)
        {
            var Product = await _db.Products.FindAsync(id);
            var Cart = await _db.Carts.FindAsync(new { User.Identity.Name, id });

            //Delete selected product if found.
            if (Product != null)
            {
                //Remove selected product.
                _db.Carts.Remove(Cart);
                Product.Stock+=Cart.Quantity;
                _db.Attach(Product).State = EntityState.Modified;

                await _db.SaveChangesAsync();

                StatusMessage = $"Product with ID: {id} removed from cart";
            }
            else
            {

                StatusMessage = $"Error: Can't find product with ID: {id}!";
            }
            //Update current page.
            return RedirectToPage();
        }
    }
}