using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using myWebApp.Pages.Product;


namespace myWebApp.Pages.Cart
{
    public class BuySecondPageModel : PageModel
    {

        private readonly AppDbContext _db;

        [BindProperty]
        public Order GivenOrder { get; set; } = new Order();
        

        public int getprice(cart c)
        {
            var price = c.Quantity * c.Product.Price;
            return price;
        }
        public int FullPrice()
        {
            int Price = 0;
            foreach (var product in Carts)
            {
                Price += product.Product.Price * product.Quantity;
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
                cartcontains = cartcontains.Where(s => s.User.Email == User.Identity.Name);
                //Check if we found anyting
                if (cartcontains.AsNoTracking().ToList().Count != 0)
                {
                    //Succes found a match
                }
                else
                {
                    StatusMessage = $"Error: No products in cart!";
                    Carts = new List<cart>();
                    //Failed no match reload page - Show all.
                    return Page();
                }
            }
            else
            {
                StatusMessage = $"Error: No account logged in, cart is empty!";
                Carts = new List<cart>();
                return Page();
                
            }
           
            Carts = await cartcontains.AsNoTracking().Include(c => c.Product).ToListAsync();
            //Update current page.
            return Page();
        }
       

        public void OnPost()
        {
            GivenOrder.FirstName = Request.Form["Orders.FirstName"];
            GivenOrder.LastName = Request.Form["Orders.LastName"];
            GivenOrder.Address = Request.Form["Orders.Address"];
            GivenOrder.Country = Request.Form["Orders.Country"];
            GivenOrder.PostalCode = Request.Form["Orders.PostalCode"];
            GivenOrder.Email = Request.Form["Orders.Email"];
            GivenOrder.Phone = Request.Form["Orders.Phone"];
            GivenOrder.cardNumber = Request.Form["Orders.cardNumber"];
            GivenOrder.experessionDate = Request.Form["Orders.experessionDate"];
        }
    }
}