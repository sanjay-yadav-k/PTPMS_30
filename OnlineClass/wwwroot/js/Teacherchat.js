"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
//document.getElementById("sendButtons").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    alert(message);
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;

    var mssag = '<div class="direct-chat-msg doted-border">' +
        '<div class="direct-chat-info clearfix">' +
        '<span class="direct-chat-name pull-left">' + user + '</span>' +
        '</div>' +

        '<div class="direct-chat-text">'
        + message +
        '</div>' +
        '</div>';
    $($(".direct-chat-messages")[0]).append(mssag);

});

connection.start().then(function () {
    document.getElementById("sendButtons").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
alert(document.getElementById("sendButtons"));
document.getElementById("sendButtons").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    alert($("#messageInput").val());
    var message = $("#messageInput").val();
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});