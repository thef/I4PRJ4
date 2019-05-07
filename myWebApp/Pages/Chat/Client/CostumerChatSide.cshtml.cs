using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myWebApp.Pages.Chat.Client
{
    [Authorize(Roles = "Admin,Costumer")]
    public class CostumerChatSideModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}