using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using myWebApp.Pages.Product;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
//namespace myWebApp.Pages.Chat

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public AppDbContext _db;

        public ChatHub(AppDbContext db)
        {
            _db = db;
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
        
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveGroupNotification", $"Someone has joined the group.", groupName);
        }

    }
}