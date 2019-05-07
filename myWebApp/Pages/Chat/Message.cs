using System;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.Pages.Chat
{
    public class Message
    {
        public Message(string Name, string Message)
        {
            UserName = Name;
            Msg = Message;
        }

        public Message()
        {

        }

        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Msg { get; set ;}
    }
}