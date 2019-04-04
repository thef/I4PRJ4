using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using System.Text;

//Because AppDbContext is writtens in this namespace...
using myWebApp.Pages.Product;
using Microsoft.EntityFrameworkCore;

namespace myWebApp.Pages.Chat.Server
{
    public class ChatServerModel : PageModel
    {
        //Database setup
        public readonly AppDbContext _db;
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

        public void OnPostStartServer()
        {
            Database ChatDB = new Database();
            Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Internetwork således at serveren fungerer på IPv4
            int _Server_PORT = 1000;
            MessageReceiver msgReceiver = new MessageReceiver(_serverSocket, _Server_PORT, ChatDB);
            msgReceiver.SetupServer();
        }

        public void OnPostTest()
        {
            Message msgtest = new Message();
            msgtest.Msg = "Goddag";
            _db.Messages.Add(msgtest);
        }

        //public void OnPostTest2()
        //{
        //    Message Message = new Message();
        //    //Message.Id = 2;
        //    Message.Msg = "hejhej ";
        //    Message.UserName = "Kaj";

        //    _db.Messages.Add(Message);
        //    _db.SaveChanges();
        //}

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