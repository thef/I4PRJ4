using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.Pages.Product
{
    public class Product
    {
        [Key]
        public int Id { get; set; } 
        
        [Required, StringLength(25)]
        public string Name { get; set; }

        [Required, StringLength(150)]
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be atleast: 0!")]
        public int Stock { get; set; }

        //[Range(0.25, double.MaxValue, ErrorMessage = "Price must be greater than: 0.25!")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public List<Cart.cart> Carts { get; set; }
    }
}