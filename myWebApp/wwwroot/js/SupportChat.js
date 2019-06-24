"use strict";

//Function invocations for startup
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Receive Message
connection.on("ReceiveMessage", function (user, message, groupName) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg;
    var SelectedArea = "TextArea" + groupName;

    document.getElementById(SelectedArea).value = document.getElementById(SelectedArea).value + encodedMsg + "\n";
});

connection.on("ReceiveErrorMessage",
    function(message, groupName) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var SelectedArea = "TextArea" + groupName;
        document.getElementById(SelectedArea).value = document.getElementById(SelectedArea).value + msg + "\n";
    });

//Receive Groupnotification
connection.on("ReceiveGroupNotification", function (message, groupName) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;

    var SelectedArea = "TextArea" + groupName;
    document.getElementById(SelectedArea).value = document.getElementById(SelectedArea).value + encodedMsg + "\n";
});

connection.start().then(function() {
    connect(1);
    connect(2);
});

//Add eventlisteners to buttons
document.getElementById("SendButton1").addEventListener("click", function () {
    sendMessage(1);
});

document.getElementById("SendButton2").addEventListener("click", function () {
    sendMessage(2);
});

document.getElementById("NextButton1").addEventListener("click", function () {
    next(1);
});

document.getElementById("NextButton2").addEventListener("click", function () {
    next(2);
});

// Get next customer in queue
function next(Selector) {
    connection.invoke("NextInQueue", Selector);
}

//Connect Function
function connect(Selector)
{
    //var groupBoxID = "groupInput" + Selector;
    //var group = document.getElementById(groupBoxID).value;
    var name = document.getElementById("userInput").value;
    connection.invoke("AddToGroup", name, Selector);
}

//sending function
function sendMessage(Selector) {
    var user = document.getElementById("userInput").value;

    //Choose correct textbox
    var messageBoxID = "messageInput" + Selector;
    var message = document.getElementById(messageBoxID).value;

    //ChooseCorrectTextarea
    var group = Selector;

    //Invoke function in hub
    connection.invoke("SendMessage", user, message, group).catch(function (err) {
        return console.error(err.toString());
    });
}