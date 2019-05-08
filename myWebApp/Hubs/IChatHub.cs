using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myWebApp.Hubs
{
    interface IChatHub
    {
        Task SendMessage(string user, string message, string groupName);
        Task AddToGroup(string name, string groupName);
    }
}
