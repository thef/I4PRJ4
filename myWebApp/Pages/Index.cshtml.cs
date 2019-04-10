using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using myWebApp.Pages.Cart;
using Microsoft.AspNetCore.Identity;

namespace myWebApp
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        private readonly UserManager<ApplicationDbUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        public List<Product> Products { get; set; }

        public List<Rating> Rates { get; set; }

        //Search funtion
        [BindProperty]
        public string Search { get; set; }

        //On Get loading page.
        public async Task OnGetAsync()
        {
            Products = await _db.Products.AsNoTracking().ToListAsync();

            Rates = await _db.Rates.AsNoTracking().ToListAsync();
        }

        //On Delete button-handler.
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var Product = await _db.Products.FindAsync(id);

            //Delete selected product if found.
            if(Product != null)
            {
                //Remove selected product.
                _db.Products.Remove(Product);

                //Delete ratings for selected product
                _db.Rates.RemoveRange(_db.Rates.Where(x => x.ProductId == id));
                await _db.SaveChangesAsync();

                StatusMessage = $"Product with ID: {id} deleted";

            } else {

                StatusMessage = $"Error: Can't find product with ID: {id}!";
            }
            //Update current page.
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);

            //Delete selected product if found.
            if (product != null)
            {
                if (User.IsInRole("Customer"))
                {
                    if (product.Stock != 0)
                    {
                        var Activeuser = _userManager.FindByEmailAsync(User.Identity.Name).Result;
                        _db.Carts.Add(new cart
                            {
                                Product = product, ProductId = id, Quantity = 1, User = Activeuser,
                                UserId = User.Identity.Name
                            }
                        );

                        product.Stock--;
                        _db.Attach(product).State = EntityState.Modified;

                        //Delete ratings for selected product
                        await _db.SaveChangesAsync();

                        StatusMessage = $"Product with ID: {id} added to cart";
                    }
                    else
                    {
                        StatusMessage = $"Error: Product with id: {id} out of stock!";
                    }
                }
            }
            else
            {

                StatusMessage = $"Error: Can't find product with ID: {id}!";
            }
            //Update current page.
            return RedirectToPage();
        }

        //On Rate button-handler.
        public async Task<IActionResult> OnPostRateAsync(int id, double rate)
        {
            //Check if rate-value is 0.0/ a radio-button value has been selected.
            if(rate <= 0.0)
            {   
                StatusMessage = $"Error: Select a value before submitting!";
            } else {

            //Find selected product from list.
            var product = await _db.Products.FindAsync(id);

                //If product DO exist!
                if(product != null)
                {
                    Rating rating = new Rating();
                    rating.ProductId = id;
                    rating.Rate = rate;
                    rating.UserName = User.Identity.Name; 

                    _db.Rates.Add(rating);
                    await _db.SaveChangesAsync();

                    StatusMessage = $"Thanks for your rating.";
                }
            }
            //Update current page.
            return RedirectToPage();
        }

        //For a defined productId find all rates, return average.
        public double OverallRating(int id)
        {
            var result = 0.0;
            var foundAmountofTimes = 0;
            foreach (var rating in Rates)
            {
                if (rating.ProductId == id)
                {
                    result += rating.Rate;
                    foundAmountofTimes ++;
                }
            }
            //Round result exsample: 12.54233565 to 12.54.
            return System.Math.Round(result = result / foundAmountofTimes, 2);
        }

        //For a defined productId tell if it exist, return true or false.
        public bool RatedYet(int id)
        {
            var result = false;
            foreach (var rating in Rates)
            {
                if(rating.ProductId == id)
                {
                    result = true;

                    //Jump out if we found a rating.
                    break;

                } else {

                    result = false;
                }
            }
            //Return statment.
            return result;
        }

        //Find number of ratings for selected product, return count.
        public double NumberOfVotes(int id)
        {
            var result = 0.0;
            foreach (var rating in Rates)
            {
                if(rating.ProductId == id)
                {
                    result ++;
                }
            }
            //Return count.
            return result;
        }

        //For a defined productId tell if current User has rated or not, return true or false.
        public bool UserHasRatedProduct(int id)
        {
            var result = false;
            foreach (var rating in Rates)
            {
                if(rating.Id == id)
                {
                    if (rating.UserName == User.Identity.Name)
                    {
                        result = true;

                        //Jump out if we found the rating for product by current user.
                        break;

                    } else {

                        result = false;
                    }
                }
            }
            return result;
        }

        //On Search button-handler.
        public async Task<IActionResult> OnPostSearchAsync(string searchString)
        {
            var products = from p in _db.Products
                select p;

            var ratings = from r in _db.Rates
                select r;

                if (!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.Name.Contains(searchString));

                    //Check if we found anyting
                    if (products.ToList().Count != 0)
                    {
                        StatusMessage = $"Displaying products that contains Name: '{searchString}'.";

                    } else {

                        StatusMessage = $"Error: No products contains Name: '{searchString}'.";

                        //Run OnGetAsync() a clean reload of current page.
                        return RedirectToPage();
                    }

                } else {

                    StatusMessage = $"Displaying every product.";
                }

                //Update tables
                Products = await products.AsNoTracking().ToListAsync();
                Rates = await ratings.AsNoTracking().ToListAsync();

            //Update current page.
            return Page();
        }
    }
}
