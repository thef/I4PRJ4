using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using myWebApp.Pages.Cart;

namespace myWebApp.Pages.Account
{
    public class  ApplicationDbUser : IdentityUser
    {
        public List<Cart.cart> Carts { get; set; }
    }
}