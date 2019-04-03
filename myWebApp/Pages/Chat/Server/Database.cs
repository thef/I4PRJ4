using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace myWebApp.Pages.Chat
{
    public class Database
    {
        public List<string> _ChatSession = new List<string>();
        public List<Socket> _ConnectedClients = new List<Socket>();

        public void UpdateChatSession(string newMsg)
        {
            _ChatSession.Add(newMsg);
        }

    }
}
