using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Authorization;
using System.Text;

//Because AppDbContext is writtens in this namespace...
using myWebApp.Pages.Product;
using Microsoft.EntityFrameworkCore;

namespace myWebApp.Pages.Chat.Server
{
    [Authorize(Roles="Admin,Support")]
    public class ChatServerModel : PageModel
    {
        //Database setup
        public AppDbContext _db;
        //public List<Message> Messages { get; set; }
        
        public ChatServerModel(AppDbContext db)
        {
            _db = db;
        }


        //public async Task OnGetAsync()
        //{
        //    Messages = await _db.Messages.AsNoTracking().ToListAsync();
        //}
    }
}