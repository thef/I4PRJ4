using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ChatServer
{
    class Program
    {
        ////_serverSocket is used to listen for incomming connections form clients. 
        public static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Internetwork således at serveren fungerer på IPv4
        public static int _Server_PORT = 1000;


        ////_clientSockets is a list of all sockets which are connected to the _serverSocket.
        //private static List<Socket> _clientSockets = new List<Socket>();

        ////List to hold chatmessages
        //public static List<string> ChatMessages = new List<string>();
        
        //Main program
        static void Main(string[] args)
        {
            Console.Title = "Server";

            //Create Database
            Database ChatDB = new Database();

            MessageReceiver msgReceiver = new MessageReceiver(_serverSocket,_Server_PORT, ChatDB);
            msgReceiver.SetupServer();
            

            //SetupServer();
            //Console.ReadLine(); //Close console
            //closeAllSockets(); TODO: Implementeres senere

            while (true)
            {

            }
        }

        
        //private static void SetupServer()
        //{
        //    Console.WriteLine("Setting up server...");
            
        //    //Bind IP-address and Port to _serversocket
        //    _serverSocket.Bind(new IPEndPoint(IPAddress.Any,PORT));
            
        //    //Set _serversocket to a listening state.
        //    _serverSocket.Listen(2); //Har en kø op til 2.
            
        //    //Venter på at en client tisluttes...
        //    _serverSocket.BeginAccept(AcceptCallback, null);

        //    Console.WriteLine("Server setup complete");
        //}

        //Tilføjer en ny socket til klienten der vil kommunikere. 
        //private static void AcceptCallback(IAsyncResult AR) //Bemærk at AR er resultat fra  beginAccept. todo: asp.net task, asynchron call, await. AR == returværdien for beginAccept
        //{
        //    //Connection attempt
        //    Console.WriteLine("Client is trying to connect");
        //    Socket clientSocket = _serverSocket.EndAccept(AR);
        //    Console.WriteLine("Client Connected");

        //    //Adds clientsocket to list of clientsockets
        //    _clientSockets.Add(clientSocket);

        //    //Begin to receive from the newly added socket.
        //    clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket); //Eksekverer kun ReceiveCallback når operationen er færdig.
            
        //    //Start new thread to wait for client to connect.
        //    _serverSocket.BeginAccept(AcceptCallback, null);
        //}


        //private static void ReceiveCallback(IAsyncResult AR)
        //{
        //    //Create socket 
        //    Socket socket = (Socket) AR.AsyncState;

        //    int received;

        //    try
        //    {
        //        received = socket.EndReceive(AR);
        //    }
        //    catch (SocketException)
        //    {
        //        Console.WriteLine("Client was forcefully disconnected");
        //        socket.Close();
        //        return;
        //    }
            
        //    byte[] dataBuf = new byte[received];
        //    Array.Copy(_buffer, dataBuf, received);
        //    string text = Encoding.ASCII.GetString(dataBuf);
        //    Console.WriteLine(text);
            
        //    ChatMessages.Add(text);
            
        //    //Make one long string to send
        //    string response = string.Empty;
        //    response = String.Join(";", ChatMessages.ToArray());

            


        //   // response = Console.ReadLine();

        //    //byte[] data = Encoding.ASCII.GetBytes(response);
        //    //socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

        //    //socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, null, socket);
        //}


        //private static void SendCallback(IAsyncResult AR)
        //{
        //    //Socket socket = (Socket)AR.AsyncState;

        //    //socket.EndSend(AR);

        //    int numberOfSockets = 0;
        //    foreach (Socket clientSocket in _clientSockets)
        //    {
        //        numberOfSockets++;
        //    }
        //    Console.WriteLine("Number of connected clients: " + numberOfSockets);
        //}
    }
}
