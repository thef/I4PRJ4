//using Xunit;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.SignalR;
//using Moq;
//using System.Dynamic;

//using SignalRChat;

//using Moq;



//namespace SignalRChat.Hubs
//{
//    public class UnitTestChatHub
//    {
        
//        private ChatHub uut;
//        //private IHubCallerClients MockClient;

        
//        [Fact]
//        public async Task Test1Async()
//        {
//            //Arrange
//            var uut = new ChatHub();
//            var MockClients = new Mock<IHubCallerClients>();
            
//            MockClients.Object.ca


//            string Message = "Testmessage";
//            uut.Clients = MockClients.Object;
            


//            //Act
//            await uut.AddToGroup()
//            await uut.SendMessage("Peter", Message, "1");
            
//            //Assert
//            MockClients.VerifyGet();

//        }


//        //[Fact]
//        //public void HubsAreMockableViaDynamic()
//        //{
//        //    bool sendCalled = false;
//        //    var MockClients = new Mock<IHubCallerClients>();
//        //    uut.Clients = MockClients.Object;
//        //    //dynamic all = new ExpandoObject();

//        //    MockClients.

//        //}

//        [Theory]
//        [InlineData("1","This is a testmessage")]
//        public async Task TestCustomerSendingMessage(string group, string message)
//        {

//        }

//    }
//}
