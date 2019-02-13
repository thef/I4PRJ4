using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace myWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<Product> Products { get; private set; }

        [TempData]
        public String Message { get; set; }
        
        public async Task OnGetAsync()
        {
            Products = await _db.Products.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var Product = await _db.Products.FindAsync(id);

            if(Product != null)
            {
                _db.Products.Remove(Product);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
