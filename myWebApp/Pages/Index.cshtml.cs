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

namespace myWebApp
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        private List<Product> _products = null;

        private List<Rating> _rates = null;

        public IndexModel(AppDbContext db)
        {
            _db = db;
            //Load Products and Rates once from database.
            //_rates = _db.Rates.AsNoTracking().ToList();
            //_products = db.Products.AsNoTracking().ToList();
        }

        [TempData]
        public string StatusMessage { get; set; }
 
        //Get list of products from database once.
        public List<Product> Products {
          get {
              if (_products == null) {
                _products = _db.Products.AsNoTracking().ToList();
              }
              return _products;
          }
          private set {
            _products = value;
          }
        }

        //Get list of ratings from database once.
        public List<Rating> Rates {
          get {
              if (_rates == null) {
                _rates = _db.Rates.AsNoTracking().ToList();
              }
              return _rates;
          }
          private set {
            _rates = value;
          }
        }

        //On Get loading page.
        public void OnGetAsync()
        {
            
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
        public async Task<double> OverallRating(int id)
        {
            //Get total number of ratings for selected productId.
            var numberOfRatings = await NumberOfVotes(id);

            //Get total sum of ratings for selected productId.
            double totalRatingSum = await _db.Rates.SumAsync(x => x.Rate);

            //Round result exsample: 12.54233565 to 12.54.
            return System.Math.Round(totalRatingSum / numberOfRatings, 2);
        }


        /* 
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
        */

        //For a defined productId tell if it exist, return true or false.
        public async Task<bool> RatedYet(int id)
        {
            Rating rating = await _db.Rates.FirstOrDefaultAsync(x => x.ProductId == id);

            if(rating != null)
            {
                return true;

            } else {

                return false;
            }
        }

        /* 
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
        */

        //Find number of ratings for selected product id, return count.
        public async Task<int> NumberOfVotes(int id)
        {
            //Return count.
            return await _db.Rates.Where(x => x.ProductId == id).CountAsync();
        }

        /* 
        //Find number of ratings for selected product id, return count.
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
        */

        //For a defined productId tell if current User has rated or not, return true or false.
        public async Task<bool> UserHasRatedProduct(int id)
        {
            Rating rating = await _db.Rates.FirstOrDefaultAsync(x => x.ProductId == id && x.UserName == User.Identity.Name);

            if(rating != null)
            {
                return false;

            } else {

                return true;
            }
        }

        /* 
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
            //Return result
            return result;
        }
        */

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
                    if (products.AsNoTracking().ToList().Count != 0)
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
