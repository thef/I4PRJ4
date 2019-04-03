using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace myWebApp.Pages.Chat
{
    public class MessageSender
    {
        private static string _Message;
        public static string Message
        {
            get { return Message; }
            set { _Message = value; }
        }

        public static int _EndpointPORT;
        private static Socket _SenderSocket;
        public static SocketConnector SocketConnector_;

        public MessageSender(Socket SenderSocket, int EndpointPORT, string Msg)
        {
            _EndpointPORT = EndpointPORT;
            _SenderSocket = SenderSocket;

            SocketConnector_ = new SocketConnector(_SenderSocket, _EndpointPORT);
            

            //_ServerSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
            //_SenderSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));//FUCKUP!
            
            Message = Msg;
        }

        //TODO: Refactor
        static void SendMessage()
        {
            string msg = _Message;
            byte[] buffer = Encoding.ASCII.GetBytes(msg);

            _SenderSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), _SenderSocket);
        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;

            socket.EndSend(AR);
        }

        public Action PromptUserAndSendMessageAction = () =>
        {
            SocketConnector_.LoopConnect();
            SendMessage();
        };

    }
}
