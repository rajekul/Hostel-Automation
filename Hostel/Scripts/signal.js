$(function () {
    var server = $.connection.myHub.server;
    var client = $.connection.myHub.client;
    var hub = $.connection.hub;

    //hub.logging = true;
    hub.start()
    .done(function (data) {
        alert("works");
        console.log(data);
       // server.sendMessage("Welcome Everyone");
        
        
    })
    .fail(function () {
        alert("error!!");
    });

    client.totalMember = function (value) {
        //alert("works rec");
        $('#totalmember').html(value);
    }

    $('#addmember').click(function () {
       alert("works");
        server.addMember();
        //alert("works send");
    });

    
})