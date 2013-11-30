$( document ).ready(function() {

    $("[rel='tooltip']").tooltip();

//    $.getJSON("http://www.telize.com/geoip", function(json) {
//            //console.log(JSON.stringify(json));
//            console.log(json)
//
//            ;
//        }
//    );

//    $.getJSON("http://ip.jsontest.com/", function(json) {
//            //console.log(JSON.stringify(json));
//            console.log(json);
//        }
//    );

    var socket;

    var useConnectionAlert = false;

    var connect = function(serverJson) {

        socket = io.connect('http://' + serverJson[serverJson['network_interface']] + ':1337/');

        /** CLIENT   https://github.com/LearnBoost/socket.io/wiki/Exposed-events **/

            // "connect" is emitted when the socket connected successfully
        socket.socket.on('connect', function () {
            if(useConnectionAlert) createAlert('Connected.', 'alert-success');
        });

        // disconnect" is emitted when the socket disconnected
        socket.socket.on('disconnect', function () {
            if(useConnectionAlert) createAlert('Disconnected.', 'alert-danger');
        });

        // "connect_failed" is emitted when socket.io fails to establish a connection to the server and has no more transports to fallback to
        socket.socket.on('connect_failed', function () {
            if(useConnectionAlert) createAlert('Connect Failed.', 'alert-danger');
        });

        // "error" is emitted when an error occurs and it cannot be handled by the other event types.
        socket.socket.on('error', function () {
            if(useConnectionAlert) createAlert('Connection error.', 'alert-danger');
        });

        // "reconnect_failed" is emitted when socket.io fails to re-establish a working connection after the connection was dropped
        socket.socket.on('reconnect_failed', function () {
            if(useConnectionAlert) createAlert('Reconnection failed.', 'alert-danger');
        });

        // "reconnect" is emitted when socket.io successfully reconnected to the server.
        socket.socket.on('reconnect', function () {
            if(useConnectionAlert) createAlert('Reconnected.', 'alert-success');
        });

        // "reconnecting" is emitted when the socket is attempting to reconnect with the server.
        socket.socket.on('reconnecting', function () {
            if(useConnectionAlert) createAlert('Reconnecting...', 'alert-info');
        });

        /** RESPONSES **/
        socket.on('message', function (data) {

            console.log("Response:", data);

            if(data.message) {
                messages.push(data);
                var html = '';
                for(var i=0; i < messages.length; ++i) {
                    html += '<b>' + (messages[i].username ? messages[i].username : 'Server') + ': </b>';
                    html += JSON.stringify(messages[i].message) + '<br />';
                }
                if(data.uid) {
                    $("#name").val(data.uid);
                    socket.emit('message', { uid: data.uid });
                }

                $("#content").html(html);
                $("#content").animate({ scrollTop: $("#content")[0].scrollHeight}, 200);
            } else if(data.error) {
                createAlert(data.error, 'alert-danger');
            } else if ( _.isEmpty(data)) {
                console.log("Empty message:", data);
            } else {
                createAlert(data, 'alert-danger');
                console.log("There is a problem:", data);
            }
        });

        socket.on('update-room', function (data) {

            // console.log(JSON.stringify(data));

            if(!data)
                console.log("empty data: " + data);
            if(data.Queue) {
                var html = '';
                for(var player in data.Queue) {
                    html += '<a href="#" class="list-group-item">' +  data.Queue[player] + '</a>';
                }
                $("#queue").html(html);
            }
            html = '';
            for(var i = 1; i < _.size(data); ++i) {
                html += '<a href="#" class="list-group-item">' +  Object.keys(data)[i] + '<span class="badge badge-info pull-right">' + _.size(data[Object.keys(data)[i]]) + '</span></a>';
            }
            $("#rooms").html(html);
        });
    };

    $.getJSON("http://kibotu.net/server.php", function(json) {
            connect(json);
        }
    );

    var messages = [];

    /** HELPER FUNCTIONS **/

    var createAlert = function(text, alertLevel) {
        $("#message-box").append('<div class="alert ' + alertLevel + '">' + text + ' <button type="button" class="close" data-dismiss="alert">&times;</button></div>');
    };

    /** REQUESTS **/

    $("#send").click(function() {

        var name = $("#name").val();

        if(name == "") {
            createAlert('Please type your name!', 'alert-danger');
        } else {
            var field = $("#field");
            socket.emit('send', { message:  field.val(), username: name });
            //field.val("");
        }
    });

    $("#join-game_type_1vs1").click( function() {
        socket.emit('join-game', { "game-type" : "game1vs1"});
    });

    $("#create-game_type_1vs1").click( function() {
        socket.emit('create-game', { "game-type" : "game1vs1"});
    });

    $("#leave-game").click( function() {
        socket.emit('leave-game', {});
    });

    $("#request-game-data").click( function() {
        socket.emit('request', { message: 'game-data'});
    });

    $("#join-game_type_2vs2").click( function() {
        socket.emit('event', { "game-event" : "join", "game-type" : "2vs2" });
    });

    $("#join-game_type_custom_1vs1").click( function() {
        socket.emit('event', { "game-event" : "join", "game-type" : "custom_1vs1" });
    });

    $("#join-game_type_custom_2vs2").click( function() {
        socket.emit('event', { "game-event" : "join", "game-type" : "custom_2vs2" });
    });

    $("#start-game").click( function() {
        socket.emit('event', { "game-event" : "start-game" });
    });

    $("#pause-game").click( function() {
        socket.emit('event', { "game-event" : "pause-game" });
    });

    $("#resume-game").click( function() {
        socket.emit('event', { "game-event" : "resume-game" });
    });

    $("#end-game").click( function() {
        socket.emit('event', { "game-event" : "end-game" });
    });

    $("#move-units").click( function() {
        socket.emit('event', { "game-event" : "move-units", source : 1, dest : 2, amount  : 50 });
    });

    $("#level-up").click( function() {
        socket.emit('event', { "game-event" : "level-up", skill : 1 });
    });

    $("#special-1").click( function() {
        socket.emit('event', { "game-event" : "special-1", dest: 1 });
    });

    $("#special-2").click( function() {
        socket.emit('event', { "game-event" : "special-2", dest: 1 });
    });

    $("#special-3").click( function() {
        socket.emit('event', { "game-event" : "special-3", dest: 1 });
    });

    $("#buy").click( function() {
        socket.emit('event', { "game-event" : "buy", item : 1 });
    });

    /** SERVER EVENTS **/
    $("#game-data").click( function() {
        socket.emit('event', {
            "game-event" : "game-data",
            planets : [
                { id : 1, position : [20, 20, 0] },
                { id : 2, position : [20, 170, 0] },
                { id : 3, position : [170, 20, 0] },
                { id : 1, position : [170, 100, 0] }],
            player : [{  }]  });
    });
});