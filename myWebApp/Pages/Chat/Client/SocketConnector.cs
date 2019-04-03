using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class SocketConnector
    {
        private int attempts = 0;
        public static Socket _clientSocket;
        public static int _ServerPORT;

        /// <summary>
        /// Takes a SenderSocket and connects it to an IPEndpoint
        /// </summary>
        /// <param name="SocketToConnect">The uninitialized socket that will be connected</param>
        /// <param name="ServerPORT">The port of the IPEndpoint</param>
        public SocketConnector(Socket SocketToConnect, int ServerPORT)
        {
            _clientSocket = SocketToConnect;
            _ServerPORT = ServerPORT;
        }

        public static void LoopConnect()
        {
            while (!_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Connect(IPAddress.Loopback, _ServerPORT);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Error: Connection to Server Failed");
                    //Console.Clear();
                    //Console.WriteLine("Connection attempts: " + attempts.ToString());
                }
            }
        }
    }
}
