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

namespace myWebApp.Pages.Chat.Client
{
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

        public string Testo = "I am Testo";

        void OnFoo()
        {
            GetListContents();
        }

        public string GetListContents()
        {
            string Contents = null;
            foreach (var StringVariable in TestList)
            {
                Contents = Contents + "\n" + StringVariable;
            }
            return Contents;
        }

        private readonly AppDbContext _db;

        public ChatClient(AppDbContext db)
        {
            _db = db;
        }

        public string ServerIPAddress = IPAddress.Loopback.ToString();
        //[TempData]
        //public string StatusMessage { get; set; }

        //[TempData]
        //public string ClientStatus { get; set; }

        //[BindProperty]
        //public InputModel Input { get; set; }

        //public class InputModel
        //{
        //    public string Message { get; set; }
        //    public string Log { get; set; }
        //}

        public List<Message> Messages { get; set; }

        public List<string> TestList = new List<string>();

        //On GET page load.
        public void OnGet()
        {
            //Get all messages from database.
            Messages = _db.Messages.AsNoTracking().ToList();
        }

        public void OnPostButtonTester()
        {
            TestList.Add("Hello1\n");
        }
        
        public string ReturnAString()
        {
            string testo1 = "hej med dig";
            return testo1;
        }
        
        public Task OnPostStartClient()
        {
            //Database ChatDB = new Database();
            MsgSender = new MessageSenderClient(_senderSocket, ServerPort, SenderPort, ReceiverPort);
            //Task SendMessages = Task.Run(MsgSender.PromptUserAndSendMessageAction);
            
            MsgReceiver = new MessageReceiver(_receiverSocket, ReceiverPort);
            Task ReceiveMessages = Task.Run(MsgReceiver.StartReceiverAction);

            //while (true)
            //{
            //    if (MessageReceiver.ReceivedString != null)
            //    {
            //        ReceivedMessage = ReceivedMessage + "\n" + MessageReceiver.ReceivedString;
            //    }
            //}
            return null;
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