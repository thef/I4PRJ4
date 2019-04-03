using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace myWebApp.Pages.Chat
{
    public class SocketConnector
    {
        //private int attempts = 0;
        public Socket _SocketToConnect;
        public int PORT_;
        private IPEndPoint EndPointToConnect;
        private IPEndPoint ConnectedEndpoint;
        /// <summary>
        /// Takes a SenderSocket and connects it to an IPEndpoint
        /// </summary>
        /// <param name="SocketToConnect">The uninitialized socket that will be connected</param>
        /// <param name="PORT">The port of the IPEndpoint</param>
        public SocketConnector(Socket SocketToConnect, int PORT)
        {
            _SocketToConnect = SocketToConnect;
            PORT_ = PORT;
            EndPointToConnect = new IPEndPoint(IPAddress.Loopback,PORT_);
            ConnectedEndpoint = new IPEndPoint(IPAddress.Loopback,9999);
        }

        public  void LoopConnect()
        {
            //while (!_SocketToConnect.Connected)
            while (ConnectedEndpoint.Port != EndPointToConnect.Port)
            {
                try
                {
                    _SocketToConnect.Connect(IPAddress.Loopback, PORT_);
                    ConnectedEndpoint = _SocketToConnect.RemoteEndPoint as IPEndPoint;
                }
                catch (SocketException)
                {
                    Console.WriteLine("Error: Connection to Remote IPEndpoint Failed");
                    //Console.Clear();
                    //Console.WriteLine("Connection attempts: " + attempts.ToString());
                }
            }
        }
    }
}
