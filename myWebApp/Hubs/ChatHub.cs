using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using myWebApp.Pages.Product;
using myWebApp.Hubs.CustomerQueue
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
//namespace myWebApp.Pages.Chat

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private CustomerQueue queue_ = new CustomerQueue(100);
        public AppDbContext _db;
        
        public ChatHub()//AppDbContext db)
        {
            //_db = db;
        }

        public async Task SendMessage(string user, string message, string groupName)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message, groupName);
            //Add message and user to database.
            //myWebApp.Pages.Chat.Message newMsg = new myWebApp.Pages.Chat.Message(user, message);
            //_db.Messages.Add(newMsg);
            //await _db.SaveChangesAsync();
        }

        public async Task NewConnection(string name)
        {
            s
        }

        public async Task AddToGroup(string name, string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveGroupNotification",  name + " has joined the chat.", groupName); //, groupName);
        }

        public async Task RemoveFromGroup(string name, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGroupNotification", name + " has left the chat.", groupName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            //Todo Next customer in queue
        }

        //public async Task RemoveCustomerFromGroup(string name, string groupName)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId)
        //}

    }
}