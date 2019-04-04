//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//namespace ChatClient
//{
//    class Program
//    {
//        //Specify ports for sending and receiving.
//        private static int PORT_Sender = 0;
//        private static int PORT_Receiver = 0;
//        private static int PORT_Server = 0;
//        //Socket
//        private static Socket _SenderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        private static Socket _ReceiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        //
//        private static string chatSession = string.Empty;
//        private static string name = string.Empty;


//        static void Main(string[] args)
//        {
//            PORT_Server = 1000; //Debugging.

//            Console.Write("Specify portnumber for the Sender: ");
//            PORT_Sender = Int32.Parse(Console.ReadLine());

//            Console.Write("Specify portnumber for PORT_Receiver: ");
//            PORT_Receiver = Int32.Parse(Console.ReadLine());

//            //Create messageSender
//            MessageSender MsgSender = new MessageSender(_SenderSocket, PORT_Server, PORT_Sender, PORT_Receiver);
//            Task SendMessages = Task.Run(MsgSender.PromptUserAndSendMessageAction);

//            //Create MessageReceiver
//            MessageReceiver MsgReceiver = new MessageReceiver(_ReceiverSocket, PORT_Receiver);
//            Task ReceiveMessages = Task.Run(MsgReceiver.StartReceiverAction);

//            while (true)
//            {
//            }
//        }
//    }
//}
