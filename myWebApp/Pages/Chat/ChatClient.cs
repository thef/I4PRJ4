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

//For using folders.
using myWebApp.Pages.Product;
using myWebApp.Pages.Account;

namespace myWebApp.Pages.Chat
{
    public class ChatClient : PageModel
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly AppDbContext _db;

        public ChatClient(AppDbContext db)
        {
            _db = db;
        }
        
        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string ClientStatus { get; set; }

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
            //Get all messages from database.
            Messages = _db.Messages.AsNoTracking().ToList();
        }

        //On POST page submit from button.
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) 
            {
                return Page();
            }
            
            //Add Message to Database.
            AddMsg(Input.Message);

            //Send a message to Chatserver and wait for response.
            SendLoop(Input.Message);

            //Shoe status message for user.
            StatusMessage = $"Message: {Input.Message} was save in database";

            //Update current page.
            return RedirectToPage();
        }
    
        //ChatServer Client
        private string SendLoop(string msg)
        {
            //Console.Write("Enter chatmessage: ");
            //string req = name + " says: " + Console.ReadLine();
            string req = msg;
            byte[] buffer = Encoding.ASCII.GetBytes(req);
            _clientSocket.Send(buffer);

            byte[] receivedBuf = new byte[1024];
            int rec = _clientSocket.Receive(receivedBuf);
            byte[] data = new byte[rec];
            Array.Copy(receivedBuf, data, rec);

            return Encoding.ASCII.GetString(data);        
            //Console.WriteLine("Support says: " + Encoding.ASCII.GetString(data));
        }

        public void AddMsg(string msg)
        {
            Message message = new Message();
            message.UserName = User.Identity.Name;
            message.Msg = msg;

            _db.Messages.Add(message);
            _db.SaveChangesAsync();
        }

        private void LoopConnect()
        {
            int attempts = 0;

            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    //do nothing
                    //Console.Clear();
                    ClientStatus = "Connection attempts: " + attempts.ToString();
                    //Console.WriteLine("Connection attempts: " + attempts.ToString());
                }
            }
            ClientStatus = "Connected";
            //Console.Clear();
            //Console.WriteLine("Connected");
        }
        //ChatServer Server
    }
}