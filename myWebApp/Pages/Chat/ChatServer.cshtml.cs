using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace myWebApp.Pages.Chat
{
    public class ChatServer : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Message { get; set; }
            public string Log { get; set; }
        }
        public string ClientMsg { get; set; }

        public string ServerStatus { get; set; }

        //benyt bindproperty fra HTML variabler til CS.
        public void OnPost()
        {
            Database ChatDB = new Database();
            Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Internetwork således at serveren fungerer på IPv4
            int _Server_PORT = 1000;
            MessageReceiver msgReceiver = new MessageReceiver(_serverSocket, _Server_PORT, ChatDB);
            msgReceiver.SetupServer();
        }

        public void OnGet()
        {

        }
    }
}