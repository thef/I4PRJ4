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
using System.Data;

namespace myWebApp
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IList<Product> Products { get; private set; }

        public IList<Rating> Rates { get; private set; }

        //On Get loading page.
        public void OnGet()
        {
            Products = _db.Products.AsNoTracking().ToList();

            Rates = _db.Rates.AsNoTracking().ToList();
        }
        /*
        public async Task OnGetAsync()
        {
            Products = await _db.Products.AsNoTracking().ToListAsync();

            Rates = await _db.Rates.AsNoTracking().ToListAsync();
        }
        */

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

                    StatusMessage = $"Thanks for your rating";
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
    }
}
