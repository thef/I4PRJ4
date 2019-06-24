"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var myGroup = null;
//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg;
    //var li = document.createElement("li");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
    document.getElementById("TextArea1").value = document.getElementById("TextArea1").value + encodedMsg + "\n";
});

connection.on("ReceiveGroupNumber",
    function(groupNumber) {
        myGroup = groupNumber;
    });

connection.on("ReceiveMessage_NewConnection",
    function(message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        document.getElementById("TextArea1").value = document.getElementById("TextArea1").value + msg + "\n";
    });

connection.on("ReceiveGroupNotification", function (message, groupname) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //var encodedMsg = msg;
    //var li = document.createElement("li");
    //li.textContent = encodedMsg;
    //document.getElementById("messagesList").appendChild(li);
    document.getElementById("TextArea1").value = document.getElementById("TextArea1").value + msg + "\n";
});

connection.start().then(function () {

    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("startChatButton").addEventListener("click", function(event) {
    var name = document.getElementById("usernameInput").value;
    connection.invoke("NewConnection", name);
    event.preventDefault();
});

////Connecting to group
//document.getElementById("connectButton").addEventListener("click",function(event) {
//        var group = document.getElementById("groupInput").value;
//        var name = document.getElementById("userInput").value
//        connection.invoke("AddToGroup", name, group);
//        event.preventDefault();
//});

////Disconnect form group
//document.getElementById("disconnectButton").addEventListener("click", function(event) {
//    var group = document.getElementById("groupInput").value;
//    var name = document.getElementById("userInput").value;
//    connection.invoke("RemoveFromGroup", name, group);
//    event.preventDefault();
//});

////Sending a message
document.getElementById("SendButton1").addEventListener("click", function (event) {
    var user = document.getElementById("usernameInput").value;
    var message = document.getElementById("messageInput1").value;
    //var group = document.getElementById(myGroup).value;
    connection.invoke("SendMessage", user, message, myGroup).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});