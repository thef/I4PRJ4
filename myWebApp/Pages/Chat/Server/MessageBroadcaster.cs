//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using ChatServer;

//namespace ChatServer
//{
//    public class MessageBroadcaster
//    {
//        private static string _Message;
//        public static string Message
//        {
//            get { return Message; }
//            set { _Message = value; }
//        }

//        private static Socket _clientSocket;
//        private Database _ServerDB;
//        public static SocketConnector _SocketConnector;

//        public MessageBroadcaster(Socket ClientSocket, int PORT)
//        {
//            _SocketConnector = new SocketConnector(ClientSocket, PORT);
//            _clientSocket = ClientSocket;
//            _ServerDB = ServerDB;
            
//        }

//        public void Broadcast()
//        {
//            foreach (var socket in _ServerDB._ConnectedClients)
//            {
//                //socket.Send()
//                //_SocketConnector.LoopConnect();
//            }
            
//        }

//        static void PromptUserMessage()
//        {
//            Console.WriteLine("Your Message: ");
//            Message = Console.ReadLine();
//        }

//        //TODO: Refactor
//        static void SendMessage()
//        {
//            string msg = _Message;
//            byte[] buffer = Encoding.ASCII.GetBytes(msg);
//            _clientSocket.Send(buffer);
//            _clientSocket.Close();
//        }

//        public Action PromptUserAndSendMessageAction = () =>
//        {
//            PromptUserMessage();
//            SocketConnector.LoopConnect();
//            SendMessage();
//        };

//    }
//}
