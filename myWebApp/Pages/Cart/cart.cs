using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using myWebApp;
using myWebApp.Pages.Account;
using System.ComponentModel;

namespace myWebApp.Pages.Cart
{
    public class cart
    {
        public string UserId { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product.Product Product { get; set; }

        public ApplicationDbUser User { get; set; }
    }
}
