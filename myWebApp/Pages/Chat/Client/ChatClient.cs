using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Net.WebSockets;
using Microsoft.AspNetCore.WebSockets;

//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;
using Microsoft.AspNetCore.Authorization;

namespace myWebApp.Pages.Chat.Client
{
    [Authorize(Roles = "Admin,Customer")]
    public class ChatClient : PageModel
    {
        private readonly AppDbContext _db;

        public ChatClient(AppDbContext db)
        {
            _db = db;
        }

        //public void OnGet()
        //{
        //    //Get all messages from database.
        //    Messages = _db.Messages.AsNoTracking().ToList();
        //}

    }
}