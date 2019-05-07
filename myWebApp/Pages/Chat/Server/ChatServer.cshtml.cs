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
        public List<Message> Messages { get; set; }
        
        public Database ChatDB;

        public ChatServerModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Message { get; set; }
            public string Log { get; set; }
        }

        public Task OnPostStartServer()
        {
            Database ChatDB = new Database();
            Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Internetwork således at serveren fungerer på IPv4
            int _Server_PORT = 1000;
            MessageReceiver msgReceiver = new MessageReceiver(_serverSocket, _Server_PORT, _db);
            msgReceiver.SetupServer();

            while (true)
            {
                //Never end thread. Server needs Database Context.
            }

            return null;
        }

        public void OnPostTest()
        {
            Message msgtest = new Message();
            msgtest.Msg = "Goddag";
            _db.Messages.Add(msgtest);
        }

        public async Task<IActionResult> OnPostTest2Async()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message Message = new Message();

            Message.Msg = "hejhej ";
            Message.UserName = "Kaj";

            _db.Messages.Add(Message);
            await _db.SaveChangesAsync();

            return RedirectToPage("/Chat/ChatServer");
        }

        public async Task OnGetAsync()
        {
            Messages = await _db.Messages.AsNoTracking().ToListAsync();
        }
    }
}