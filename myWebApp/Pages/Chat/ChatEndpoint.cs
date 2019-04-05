using System;
using System.ComponentModel.DataAnnotations;

namespace myWebApp.Pages
{
    public class ChatEndpoint
    {
        [Key]
        public string IPAdress { get; set; }
        public int Port { get; set; }
    }
}
