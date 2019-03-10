$(function () {
    var server = $.connection.myHub.server;
    var client = $.connection.myHub.client;
    var hub = $.connection.hub;

    //hub.logging = true;
    hub.start()
    .done(function (data) {
        
        console.log(data);
        alert("error!!");
       // server.sendMessage("Welcome Everyone");
        server.addMember();
        
    })
    .fail(function () {
        alert("error!!");
    });

    client.totalMember = function (value) {
        $('#totalmember').html(value);
    }

    $('#addbutton').click(function () {
       // alert("works");
        server.addMember();
    });

    
})