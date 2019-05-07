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
    [Authorize(Roles = "Admin,Costumer")]
    public class ChatClient : PageModel
    {
        private static Socket _senderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Socket _receiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static MessageSenderClient MsgSender;
        private static MessageReceiver MsgReceiver;

        [BindProperty] public int SenderPort { get; set; }
        [BindProperty] public int ReceiverPort { get; set; }
        [BindProperty] public int ServerPort { get; set; }
        [BindProperty] public string Message { get; set; }
        [BindProperty] public string ReceivedMessage { get; set; }

        private readonly AppDbContext _db;

        public ChatClient(AppDbContext db)
        {
            _db = db;
        }

        public string ServerIPAddress = IPAddress.Loopback.ToString();

        public List<Message> Messages { get; set; }

        public List<string> TestList = new List<string>();

        //On GET page load.
        public void OnGet()
        {
            //Get all messages from database.
            Messages = _db.Messages.AsNoTracking().ToList();
        }

        public Task OnPostSendMessage()
        {
            MessageSenderClient.Message = "hej fra client";
            Task SendMessages = Task.Run(MsgSender.PromptUserAndSendMessageAction);
            ReceivedMessage = ReceivedMessage + "\n" + MessageReceiver.ReceivedString;
            //MsgSender.PromptUserAndSendMessageAction
            return null;
        }

    }
}