using System.ComponentModel.DataAnnotations;

namespace myWebApp
{
    public class Product
    {
         public int Id { get; set; } 
        
         [Required, StringLength(25)]
         public string Name { get; set; }

        [Required, StringLength(40)]
         public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be atleast: 0!")]
         public int Stock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than: 0!")]
         public int Price { get; set; }
    }
}