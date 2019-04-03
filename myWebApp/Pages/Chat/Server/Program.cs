//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;


//namespace ChatServer
//{
//    class Program
//    {
//        ////_serverSocket is used to listen for incomming connections form clients. 
//        public static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Internetwork således at serveren fungerer på IPv4
//        public static int _Server_PORT = 1000;


//        ////_clientSockets is a list of all sockets which are connected to the _serverSocket.
//        //private static List<Socket> _clientSockets = new List<Socket>();

//        ////List to hold chatmessages
//        //public static List<string> ChatMessages = new List<string>();

//        //Main program
//        static void Main(string[] args)
//        {
//            Console.Title = "Server";

//            //Create Database
//            Database ChatDB = new Database();

//            MessageReceiver msgReceiver = new MessageReceiver(_serverSocket, _Server_PORT, ChatDB);
//            msgReceiver.SetupServer();

//            while (true)
//            {

//            }
//        }
//    }
//}
