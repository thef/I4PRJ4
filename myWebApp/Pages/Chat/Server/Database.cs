using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatServer
{
    public class Database
    {
        public List<string> _ChatSession = new List<string>();
        public List<Socket> _ConnectedClients = new List<Socket>();

        public void UpdateChatSession(string newMsg)
        {
            _ChatSession.Add(newMsg);
        }

        //public void UpdateConnectedClients(Socket ClientSocket)
        //{
        //    _ConnectedClients.Add(ClientSocket);
        //}

    }
}
