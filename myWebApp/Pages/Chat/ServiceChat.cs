//For using folders.
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myWebApp.Pages.Chat
{
    public class ServiceChat : PageModel
    {
        public List<string> log = new List<string>();

        /*
        //List of logs in string format.
        public List<string> log { get; set; }
        */

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Message { get; set; }
            public string Log { get; set; }
        }
        
        //On GET page load.
        public void OnGet()
        {

        }

        //On POST page submit from button.
        public async Task<IActionResult> OnPost()
        {
            StatusMessage = $"Message: {Input.Message}";

            log.Add("{Input.Message}");

            //Update current page.
            return Page();
        }
    }
}