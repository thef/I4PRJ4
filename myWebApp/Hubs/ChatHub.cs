using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using myWebApp.Pages.Product;
using myWebApp.Hubs.CustomerQueue;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
//namespace myWebApp.Pages.Chat

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        public static CustomerQueue queue_ = new CustomerQueue(100);
        public AppDbContext _db;
        
        public ChatHub()//AppDbContext db)
        {
            
            //_db = db;
        }

        public async Task SendMessage(string user, string message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message, groupName);
        }

        public async Task NewConnection(string name)
        {
           queue_.Enqueue(name,Context.ConnectionId);
           await Clients.Caller.SendAsync("ReceiveMessage_NewConnection", "Hello " + name + ", you are now in queue");
        }

        public async Task NextInQueue(string groupName)
        {
            try
            {
                Customer nextCustomer = queue_.Dequeue();
                await Groups.AddToGroupAsync(nextCustomer.connectionID_, groupName);

                //Send groupnumber to client
                await Clients.Client(nextCustomer.connectionID_).SendAsync("ReceiveGroupNumber", groupName);
                await Clients.Group(groupName).SendAsync("ReceiveGroupNotification",
                    nextCustomer.name_ + "has joined the chat.", groupName);
            }
            catch
            {
                await Clients.Caller.SendAsync("ReceiveErrorMessage", "ERROR: Queue is empty", groupName);
            }
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