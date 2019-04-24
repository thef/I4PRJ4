using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using myWebApp.Pages.Product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace myWebApp.Pages.Cart
{
    public class BuyFirstPageModel : PageModel
    {
        
        private readonly AppDbContext _db;

        public BuyFirstPageModel(AppDbContext db)
        {
            _db = db;
        }

        [TempData]
        public String StatusMessage { get; set; }
        [BindProperty]
        public Order Orders { get; set; }

        public class Order
        {
            public int orderId { get; set; }
            public System.DateTime OrderDateTime { get; set; }
            public string Username { get; set; }

            [Required(ErrorMessage = "First Name is required")]
            [DisplayName("First Name")]
            [StringLength(160)]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is required")]
            [DisplayName("Last Name")]
            [StringLength(160)]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Address is required")]
            [StringLength(70)]
            public string Address { get; set; }

            [Required(ErrorMessage = "Postal Code is required")]
            [DisplayName("Postal Code")]
            [StringLength(10)]
            public string PostalCode { get; set; }

            [Required(ErrorMessage = "Country is required")]
            [StringLength(40)]
            public string Country { get; set; }

            [StringLength(24)]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Email Address is required")]
            [DisplayName("Email Address")]
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
                ErrorMessage = "Email is is not valid.")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required(ErrorMessage = "Card Number is required")]
            [DisplayName("Card Number")]
            [RegularExpression(@"[[z0-9]{4} -[z0-9]{4} -[z0-9]{4} -[z0-9]{4}", ErrorMessage = "Invalid Card Number")]
            public int cardNumber { get; set; }

            [Required(ErrorMessage = "Expression date is required")]
            [DisplayName("Expression date")]
            [RegularExpression(@"[[z0-9]{2} -[z0-9]{2}]", ErrorMessage = "Invalid Expression date")]
            public int experessionDate { get; set; }

            [Required(ErrorMessage = "CVC number required")]
            [DisplayName("CVC number")]
            [RegularExpression(@"[[[z0-9]{3}]", ErrorMessage = "Invalid CVC number")]
            public int cvc { get; set; }

            [ScaffoldColumn(false)]
            public decimal Total { get; set; }

            [ScaffoldColumn(false)]
            public string PaymentTransactionId { get; set; }

            [ScaffoldColumn(false)]
            public bool HasBeenShipped { get; set; }

            public List<OrderDetail> OrdersdDetails { get; set; }
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _db.Products.Add(Order);
        //    await _db.SaveChangesAsync();

        //    StatusMessage = $"Product: {Orders.FirstName} added!";

        //    return RedirectToPage("/Index");
        //}
    }
    
}