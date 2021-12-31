"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/RawParam").build();

connection.on("RawDataCome", function (message) {
    let li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${message}`;
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});
