using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System.Dynamic;

using SignalRChat;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using myWebApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Internal;

namespace SignalRChat.Hubs
{
    public class UnitTestChatHub
    {
        [Fact]
        public async Task AddUserToGroupTest()
        {
            //*****************
            //***ARRANGE*******
            //*****************
            var uut = new ChatHub();

            //Message Setup
            string UserName = "TestUser";
            string Message = "TestMessage";
            string Group = "TestGroup";
            object[] argumentsObj = { UserName, Message, Group };

            // Mocks setup
            var MockClients = new Mock<IHubCallerClients>();
            var MockClientProxy = new Mock<IClientProxy>();
            var MockClientContext = new Mock<HubCallerContext>();
            var MockClientGroupManager = new Mock<IGroupManager>();

            // Mock Setup
            MockClients.Setup(mc => mc.Group(Group)).Returns(MockClientProxy.Object);

            // Property Injection
            uut.Clients = MockClients.Object;
            uut.Context = MockClientContext.Object;
            uut.Groups = MockClientGroupManager.Object;
            
            //*****************
            //***ACT***********
            //*****************
            await uut.AddToGroup(UserName, Group);
            
            //*****************
            //***ASSERT********
            //*****************
            MockClients.Verify(MC => MC.Group(Group), Times.Once);

            MockClientProxy.Verify(mcp => mcp.SendCoreAsync(
                "ReceiveGroupNotification", 
                It.Is<object[]>(o => (string)o[0] == UserName + " has joined the chat." && (string)o[1] == Group),
                default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public async Task BroadcastMessageToGroupTest()
        {
            //*****************
            //***ARRANGE*******
            //*****************
            var uut = new ChatHub();

            //Message Setup
            string UserName = "TestUser";
            string Message = "TestMessage";
            string Group = "TestGroup";
            object[] argumentsObj = { UserName, Message, Group };

            // Mocks setup
            var MockClients = new Mock<IHubCallerClients>();
            var MockClientProxy = new Mock<IClientProxy>();
            var MockClientContext = new Mock<HubCallerContext>();
            var MockClientGroupManager = new Mock<IGroupManager>();

            // Mock Setup
            MockClients.Setup(mc => mc.Group(Group)).Returns(MockClientProxy.Object);

            // Property Injection
            uut.Clients = MockClients.Object;
            uut.Context = MockClientContext.Object;
            uut.Groups = MockClientGroupManager.Object;

            // Add 3 Clients to group
            await uut.AddToGroup(UserName, Group);
            await uut.AddToGroup(UserName, Group);
            await uut.AddToGroup(UserName, Group);

            //*****************
            //***ACT***********
            //*****************
            await uut.SendMessage(UserName, Message, Group);

            //*****************
            //***ASSERT********
            //*****************
            MockClients.Verify(MC => MC.Group(Group), Times.Exactly(4));

            MockClientProxy.Verify(mcp => mcp.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => (string)o[0] == UserName && (string)o[1] == Message && (string)o[2] == Group),
                    default(CancellationToken)),
                    Times.Once);
        }

        [Fact]
        public async Task RemoveFromGroupTest()
        {
            //*****************
            //***ARRANGE*******
            //*****************
            var uut = new ChatHub();

            //Message Setup
            string UserName = "TestUser";
            string Message = "TestMessage";
            string Group = "TestGroup";
            object[] argumentsObj = { UserName, Message, Group };

            // Mocks setup
            var MockClients = new Mock<IHubCallerClients>();
            var MockClientProxy = new Mock<IClientProxy>();
            var MockClientContext = new Mock<HubCallerContext>();
            var MockClientGroupManager = new Mock<IGroupManager>();

            // Mock Setup
            MockClients.Setup(mc => mc.Group(Group)).Returns(MockClientProxy.Object);

            // Property Injection
            uut.Clients = MockClients.Object;
            uut.Context = MockClientContext.Object;
            uut.Groups = MockClientGroupManager.Object;

            // Add 1 Client to group
            //await uut.AddToGroup(UserName, Group);

            //*****************
            //***ACT***********
            //*****************
            await uut.RemoveFromGroup(UserName, Group);

            //*****************
            //***ASSERT********
            //*****************
            MockClients.Verify(MC => MC.Group(Group), Times.Exactly(2));

            MockClientProxy.Verify(mcp => mcp.SendCoreAsync(
                    "ReceiveGroupNotification",
                    It.Is<object[]>(o => (string)o[0] == UserName + " has left the chat."),
                    default(CancellationToken)),
                Times.Once);
        }
    }

}