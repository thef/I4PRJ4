using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class MessageSender
    {
        private static string _Message;
        public static string Message
        {
            get { return Message; }
            set { _Message = value; }
        }

        public static int _SenderPORT;
        public static int _ServerPORT;
        public static int _ReceiverPORT;
        private static Socket _SenderSocket;
        public static SocketConnector SocketConnector_;

        /// <summary>
        /// Constructor: Uses a SocketConnector-object to connect a sender-socket to a remote IPEndpoint.
        /// </summary>
        /// <param name="SenderSocket">The socket which is used to send to a remote IPEndpoint. Socket must not be initialized.</param>
        /// <param name="PORT">TODO: Change from integer to IPEndpoiont. The specifiec PORT in an IPAddress</param>
        /// <param name="ReceiverPORT">The Port to the receiver for this host</param>
        //public MessageSender(Socket SenderSocket, int PORT, int ReceiverPORT)
        public MessageSender(Socket SenderSocket, int ServerPORT, int SenderPORT, int ReceiverPort)
        {
            _ServerPORT = ServerPORT;
            _SenderPORT = SenderPORT;
            _SenderSocket = SenderSocket;
            _ReceiverPORT = ReceiverPort;
            SocketConnector_ = new SocketConnector(_SenderSocket, _ServerPORT);
        }

        static void PromptUserMessage()
        {
            Message = Console.ReadLine();
        }

        static void SendMessage()
        {
            string msg = _Message + ";" + _ReceiverPORT.ToString();
            byte[] buffer = Encoding.ASCII.GetBytes(msg);
            _SenderSocket.Send(buffer);
           
            //Create new instance
            MessageSender MsgSender = new MessageSender(_SenderSocket, _ServerPORT, _SenderPORT, _ReceiverPORT);
            Task SendMessages = Task.Run(MsgSender.PromptUserAndSendMessageAction);
            
        }

        public Action PromptUserAndSendMessageAction = () =>
        {
            PromptUserMessage(); 
            SocketConnector.LoopConnect();
            SendMessage();
        };
        
    }
}
