﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

//
connection.on("ReceiveGroupNotification", function (message, groupname) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});


//
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//Connecting to group
document.getElementById("connectButton").addEventListener("click",function(event) {
        var group = document.getElementById("groupInput").value;
        var name = document.getElementById("userInput").value
        connection.invoke("AddToGroup", name, group);
        event.preventDefault();
});

//Disconnect form group
document.getElementById("disconnectButton").addEventListener("click", function(event) {
    var group = document.getElementById("groupInput").value;
    var name = document.getElementById("userInput").value;
    connection.invoke("RemoveFromGroup", name, group);
    event.preventDefault();
});

//Sending a message
document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var group = document.getElementById("groupInput").value;
    connection.invoke("SendMessage", user, message, group).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});