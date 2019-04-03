using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using myWebApp;

namespace myWebApp.Pages.Cart
{
    public class cart
    {
        [Key]
        public string ItemId { get; set; }

        public string UserId { get; set; }

        public int Quantity { get; set; }

        public  int ProductId { get; set; }

        public Product.Product Product { get; set; }
    }
}
