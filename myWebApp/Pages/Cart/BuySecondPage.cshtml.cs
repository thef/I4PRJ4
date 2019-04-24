using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myWebApp.Pages.Cart
{
    public class BuySecondPageModel : PageModel
    {
        [BindProperty]
        public Order GivenOrder { get; set; } = new Order();

 

        
        public void OnGet()
        {
            GivenOrder.FirstName = "SHIT v2";

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