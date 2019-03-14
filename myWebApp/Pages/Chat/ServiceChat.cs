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

namespace myWebApp.Pages.Chat
{
    public class ServiceChat : PageModel
    {
        private readonly AppDbContext _db;

        public ServiceChat(AppDbContext db)
        {
            _db = db;
        }
        
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Message { get; set; }
            public string Log { get; set; }
        }

        public List<Message> Messages { get; set; }
        
        //On GET page load.
        public void OnGet()
        {
            Messages = _db.Messages.AsNoTracking().ToList();

        }

        //On POST page submit from button.
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            Message message = new Message();
            message.UserName = User.Identity.Name;
            message.Msg = Input.Message;

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            StatusMessage = $"Message: {Input.Message} was save in database";

            //Update current page.
            return RedirectToPage();
        }
    }
}