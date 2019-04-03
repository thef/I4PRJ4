using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatServer
{
    class ChatRoom
    {
        private List<IPEndPoint> _EndpointList;
        private List<Socket> _SenderSockets;

        ChatRoom()
        {
            _EndpointList = new List<IPEndPoint>();
            _SenderSockets = new List<Socket>();
        }

        void ConnectNewClient(IPEndPoint Endpoint)
        {

        }

        void BroadcastMsg(string msg)
        {
            //_EndpointList.Add();
        }
    }
}
