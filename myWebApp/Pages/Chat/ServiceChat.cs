using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace myWebApp.Pages.Chat
{
    public class ServiceChat : PageModel
    {
        //List of Messages in string format.
        public IList<string> Messages { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Message { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {

            //Create list of User.
            List<string> listMessages = new List<string>();
            Messages = listMessages;

            //Update current page.
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            StatusMessage = $"Message: {Input.Message}";

            Messages.Add(Input.Message.ToString());

            //Update current page.
            return RedirectToPage();
        }

    }
}