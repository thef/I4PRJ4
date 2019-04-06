﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using myWebApp.Pages.Product;

namespace myWebApp.Pages.Chat.Client
{
    public class MessageReceiver
    {
        private static Socket _ReceiverSocket;
        private static int _PORT;
        private string Msg = string.Empty;
        
        //Buffer, Buffersize, and Ports for setup
        private const int BUFFER_SIZE = 1000;
        private static byte[] _buffer = new byte[1000];
        private readonly AppDbContext _db;

        public static string ReceivedString = null;

        public MessageReceiver(Socket ServerSocket, int PORT)
        {
            _ReceiverSocket = ServerSocket;
            _PORT = PORT;
        }

        public Action StartReceiverAction = () => { SetupServer(); };

        public static void SetupServer()
        {
            _ReceiverSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
            _ReceiverSocket.Listen(1);
            _ReceiverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Receiver-setup complete");
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
           Socket AcceptSocket = _ReceiverSocket.EndAccept(AR);
           AcceptSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, AcceptSocket);
           _ReceiverSocket.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket ReceiverSocket = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = ReceiverSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client was forcefully disconnected");
                ReceiverSocket.Close();
                return;
            }

            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf);

            //Console.WriteLine("Message Received: " + text);
            ReceivedString = text;
            
            ReceiverSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, ReceiverSocket);
        }


    }
}
