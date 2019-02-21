using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace myWebApp.Pages.Product
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;

        public EditModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _db.Products.FindAsync(id);
            if(Product == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _db.Attach(Product).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Product {Product.Id} not found!", e);
            }

            return RedirectToPage("/Index");
        }

    }
}