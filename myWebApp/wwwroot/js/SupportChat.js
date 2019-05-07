"use strict";

//Function invocations for startup
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Connect to groups

//Disable send button until connection is established
//document.getElementById("SendButton1").disabled = true;

//Receive Message
connection.on("ReceiveMessage", function (user, message, groupName) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    
    //var SelectedList = "List" + groupName;
    var SelectedArea = "TextArea" + groupName;

    document.getElementById(SelectedArea).value = document.getElementById(SelectedArea).value + encodedMsg + "\n";
        //appendChild(encodedMsg);
    //document.getElementById(SelectedList).appendChild(li);
});


//Receive Groupnotification
connection.on("ReceiveGroupNotification", function (message, groupName) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("List"+groupName).appendChild(li);
});

connection.start().then(function() {
    connect(1);
    connect(2);
});
//    document.getElementById("SendButton1").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());

////    document.getElementById("SendButton2").disabled = false;
////}).catch(function (err) {
////    return console.error(err.toString());
//});

//document.getElementById("ConnectButton1").addEventListener("click", function (event) {
//    var group = document.getElementById("groupInput1").value;
//    connection.invoke("AddToGroup", group);
//    event.preventDefault();
//});

//document.getElementById("SendButton1").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput1").value;
//    var group = document.getElementById("groupInput1").value;
//    connection.invoke("SendMessage", user, message, group).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

//Add eventlisteners to buttons
document.getElementById("SendButton1").addEventListener("click", function () {
    sendMessage(1);
});

//document.getElementById("ConnectButton1").addEventListener("click", function () {
//    connect(1);
//});

document.getElementById("SendButton2").addEventListener("click", function () {
    sendMessage(2);
});

//document.getElementById("ConnectButton2").addEventListener("click", function () {
//    connect(2);
//});
////document.getElementById("SendButton2").addEventListener("click", sendMessage(2));

//Connect Function
function connect(Selector)
{
    //var groupBoxID = "groupInput" + Selector;
    //var group = document.getElementById(groupBoxID).value;
    connection.invoke("AddToGroup", Selector);
}

//sending function
function sendMessage(Selector) {
    var user = document.getElementById("userInput").value;

    //Choose correct textbox
    var messageBoxID = "messageInput" + Selector;
    var message = document.getElementById(messageBoxID).value;

    //ChooseCorrectTextarea
    var group = Selector;

    ////Choose correct groupbox
    //var groupBoxID = "groupInput" + Selector;
    //var group = document.getElementById(groupBoxID).value;

    //Invoke function in hub
    connection.invoke("SendMessage", user, message, group).catch(function (err) {
        return console.error(err.toString());
    });
}

