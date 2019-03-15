using System;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.Pages.Chat
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Msg { get; set ;}
    }
}