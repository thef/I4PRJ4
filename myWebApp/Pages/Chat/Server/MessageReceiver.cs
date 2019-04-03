using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ChatServer
{
    public class MessageReceiver
    {
        //Socketsetup
        private static Socket _ServerSocket;
        private int _PORT;
        private string Msg = string.Empty; //todo: is this unused?
        
        //Buffer, Buffersize, and Ports for setup
        private const int BUFFER_SIZE = 1000;
        private static byte[] _buffer = new byte[1000];
        
        //Setup objects
        public static Database ChatSessionDB;
        public static MessageSender MsgSender;
        private static SocketConnector Connector;

        /// <summary>
        /// Constructor for MessageReceiver class. 
        /// </summary>
        /// <param name="ServerSocket">This serversocket is used as an endpoint for clients.</param>
        /// <param name="PORT">Specifies which port is used for serversocket.</param>
        /// <param name="DB">Database for ChatSessions.</param>
        public MessageReceiver(Socket ServerSocket, int PORT, Database DB)
        {
            _ServerSocket = ServerSocket;
            _PORT = PORT;
            Connector = new SocketConnector(ServerSocket,PORT);
            ChatSessionDB = DB;
        }

        /// <summary>
        /// This function is used start the server.
        /// </summary>
        public void SetupServer()
        {
            _ServerSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
            _ServerSocket.Listen(10);
            _ServerSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Server setup complete");
        }

        /// <summary>
        /// Asynchron callbackfunction for accepting incomming connections from clients.
        /// </summary>
        /// <param name="AR">This is the result form _serverSocket.BeginAccept function. WARNING: Works recursively without a basecase!</param>
        private static void AcceptCallback(IAsyncResult AR)
        {
            //TODO: add list to database instead
            Socket clientSocket = _ServerSocket.EndAccept(AR);
            Console.WriteLine("A new client is Connected");
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
            _ServerSocket.BeginAccept(AcceptCallback, null);
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
            string[] inputs = text.Split(';');
            Console.WriteLine("Text Received: " + inputs[0]);
            Console.WriteLine("Port to send: " + inputs[1]);

            // = Encoding.ASCII.GetString(inputs[0]);

            IPEndPoint remoteIpEndPoint = ReceiverSocket.RemoteEndPoint as IPEndPoint;
            remoteIpEndPoint.Port = Int32.Parse(inputs[1]);
            
            bool doesIPEndPointExist = false;
            foreach (var socket in ChatSessionDB._ConnectedClients)
            {
                IPEndPoint test = socket.RemoteEndPoint as IPEndPoint;
                //if RemoteEndPoint exist...
                if (test.Port == remoteIpEndPoint.Port)
                {
                    doesIPEndPointExist = true;
                    break;
                }
            }

            if (!doesIPEndPointExist)
            {
                Socket SenderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ChatSessionDB._ConnectedClients.Add(SenderSocket);
                ChatSessionDB._ConnectedClients[ChatSessionDB._ConnectedClients.Count-1].Connect(IPAddress.Loopback,remoteIpEndPoint.Port);
            }
            
            foreach (var socket in ChatSessionDB._ConnectedClients)
            {
                if (!socket.Connected)
                {
                    Console.WriteLine("Socket not connected!");
                    continue;
                }
                else
                {
                    socket.Send(dataBuf);
                }
            }
            ReceiverSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, ReceiverSocket);
        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
    }
}

