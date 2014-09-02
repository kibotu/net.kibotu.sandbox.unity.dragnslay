$( document ).ready(function() {

    $("[rel='tooltip']").tooltip();

    var sl = 12; // substring length to shorten names
    var os_stats_update_interval = 30000;

    Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });

    var options = {
        chart: {
        type: 'spline',
        animation: Highcharts.svg, // don't animate in old IE
        marginRight: 10,
        events: {
            load: function() {

//                var series = this.series[0];
//                setInterval(function() {
//
//                    var x = (new Date()).getTime(), // current time
//                    y = 0;
//                    series.addPoint([x, y], true, true);
//                }, 1000);
            }
        }
    },
    title: {
        text: 'Current Ping Rate'
    },
    xAxis: {
        type: 'datetime',
            tickPixelInterval: 100
    },
    yAxis: {
        title: {
            text: 'Milliseconds'
        },
        plotLines: [{
            value: 0,
            width: 1,
            color: '#808080'
        }]
    },
    tooltip: {
        formatter: function() {
            return '<b>'+ this.series.name +'</b><br/>'+
                Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) +'<br/>'+
                Highcharts.numberFormat(this.y, 2);
        }
    },
    legend: {
        enabled: false
    },
    exporting: {
        enabled: false
    },
    series: [{
        name: 'Ping',
        marker: {
            radius: 3
        },
        lineWidth: 2,
        data: (function() {
            // generate an array of random data
            var data = [],
                time = (new Date()).getTime(),
                i;

//            data.push(time, 0);

            for (i = -100; i <= 0; i++) {
                data.push({
                    x: time + i * 1000,
                    y: 0
                });
            }
            return data;
        })()
    }]};


    // var chart =  new Highcharts.Chart(options);
    $('.glview').highcharts(options);

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


    String.prototype.replaceAll = function( token, newToken, ignoreCase ) {
        var _token;
        var str = this + "";
        var i = -1;

        if ( typeof token === "string" ) {

            if ( ignoreCase ) {

                _token = token.toLowerCase();

                while( (
                    i = str.toLowerCase().indexOf(
                        token, i >= 0 ? i + newToken.length : 0
                    ) ) !== -1
                    ) {
                    str = str.substring( 0, i ) +
                        newToken +
                        str.substring( i + token.length );
                }

            } else {
                return this.split( token ).join( newToken );
            }

        }
        return str;
    };

    var socket;

    var useConnectionAlert = false;
    var receivedPackages = [];

    var connect = function(serverJson) {

        socket = io('http://' + serverJson[serverJson['network_interface']] + ':' + serverJson['tcp_port']);

        /** CLIENT   https://github.com/LearnBoost/socket.io/wiki/Exposed-events **/

            // "connect" is emitted when the socket connected successfully
        socket.on('connect', function () {
            if(useConnectionAlert) createAlert('Connected.', 'alert-success');
        });

        // disconnect" is emitted when the socket disconnected
        socket.on('disconnect', function () {
            if(useConnectionAlert) createAlert('Disconnected.', 'alert-danger');
        });

        // "connect_failed" is emitted when socket.io fails to establish a connection to the server and has no more transports to fallback to
        socket.on('connect_failed', function () {
            if(useConnectionAlert) createAlert('Connect Failed.', 'alert-danger');
        });

        // "error" is emitted when an error occurs and it cannot be handled by the other event types.
        socket.on('error', function () {
            if(useConnectionAlert) createAlert('Connection error.', 'alert-danger');
        });

        // "reconnect_failed" is emitted when socket.io fails to re-establish a working connection after the connection was dropped
        socket.on('reconnect_failed', function () {
            if(useConnectionAlert) createAlert('Reconnection failed.', 'alert-danger');
        });

        // "reconnect" is emitted when socket.io successfully reconnected to the server.
        socket.on('reconnect', function () {
            if(useConnectionAlert) createAlert('Reconnected.', 'alert-success');
        });

        // "reconnecting" is emitted when the socket is attempting to reconnect with the server.
        socket.on('reconnecting', function () {
            if(useConnectionAlert) createAlert('Reconnecting...', 'alert-info');
        });


        var isInGame = false;

        /** RESPONSES **/
        socket.on('message', function (data) {

            console.log("Response:", data);

            if(data.message) {

                if(data.message == 'ping') {
                    socket.emit('pong');
                    return;
                }

                if(data.message == 'latency') {
                    $("#latency").html("[" + data.latency + " ms]");

                    var chart = $('.glview').highcharts();

                    chart.series[0].addPoint([ (new Date()).getTime(), data.latency], true, true);
                    return;
                }

                if(data.message == 'turn-done') {
                    data.playeruid = $("#name").val();
                    if(receivedPackages.length > 0) {
                        data.packages = receivedPackages;
                        receivedPackages = [];
                    }
                    socket.emit('turn-done', data);
                    return;
                }

                if( data.message == 'spawn-unit' ||
                    data.message == 'move-unit' ||
                    data.message == 'unit-arrival') {
                    receivedPackages.push(data.packageId);
                    return;
                }

                messages.push(data);
                var html = '';
                for(var i=0; i < messages.length; ++i) {
                    html += '<b>' + (messages[i].username ? messages[i].username : 'Server') + ': </b>';
                    html += JSON.stringify(messages[i].message) + '<br />';
                }

                if(data.uid) {
                    $("#name").val(data.uid);
                    socket.emit('message', { uid: data.uid });
                    requestOsStats();
                }

                $("#content").html(html);
                $("#content").animate({ scrollTop: $("#content")[0].scrollHeight}, 200);

                if(data.message == 'server-game-ready') {
                    sendRequestGameData();
                }

                if(data.message == 'game-data') {
                    socket.emit('client-game-ready', {});
                }

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
                for(var player in data.Queue.user) {
                    html += '<a href="#" title="' + data.Queue.user[player] + '" class="list-group-item">' +  data.Queue.user[player].substring(0,sl) + '...</a>';
                }
                $("#queue").html(html);
            }
            html = '';
            for(var i = 1; i < _.size(data); ++i) {
                // roomName = Object.keys(data)[i]
                // amountUserInRoom = _.size(data[Object.keys(data)[i]])
                // userNamesInRoom = data[Object.keys(data)[i]]

                html += '<a href="#" title="' + Object.keys(data)[i] + '" data-content="' + data[Object.keys(data)[i]].user + '" class="list-group-item room-list">' +  Object.keys(data)[i].substring(0,sl) + '...<span class="badge badge-info pull-right">' + _.size(data[Object.keys(data)[i]].user) + '</span></a>';
            }
            $("#rooms").html(html);
        });

        socket.on('os-stats', function(data) {
            //console.log(data);
            var cpuUsage = data.cpuUsage * 100;
            var memoryUsage = 100 - data.freememPercentage * 100;
            $('#cpu-usage').attr('style', "width:" +  cpuUsage + '%');
            $('#cpu-usage').addClass(getProgressColorClass(cpuUsage));
            $('#memory-usage').attr('style', "width:" + memoryUsage + '%');
            $('#memory-usage').addClass(getProgressColorClass(memoryUsage));
            $('#connected-clients').html('[' + data.clients + ' connected Clients]');
        });
    };

    var getProgressColorClass = function(percentatge) {
        var colorClass = "progress-bar-success";
        if(percentatge >= 33 && percentatge < 66) {
            colorClass = "progress-bar-warning";
        } else if(percentatge >= 66){
            colorClass = "progress-bar-danger";
        }
        return colorClass;
    };

    $("#rooms").click(function(event) {
        event.preventDefault();
        var kids = $( event.target ).children();
        var link = kids['context'];
        var data = $(link).attr('data-content');
        var usernames = data.trim().split(',');
        var output = "";

        _.each(usernames, function(user) {
            output += '<a title="' + user + '" href="#" class="list-group-item">' + user.substring(0,sl) + '...</a>';
        });
       //var usernames = daita.replaceAll(',', '<br />');
        $("#user").html(output);
    });

    $.getJSON("http://kibotu.net/server.php", function(json) {
        connect(json);
    });

    var requestOsStats = function() {
        socket.emit('os-stats', {});
    };

    setInterval(function() {
        requestOsStats();
    },os_stats_update_interval);

    var messages = [];

    /** HELPER FUNCTIONS **/

    var createAlert = function(text, alertLevel) {
        $("#message-box").append('<div class="alert ' + alertLevel + '">' + text + ' <button type="button" class="close" data-dismiss="alert">&times;</button></div>');
    };

    /** REQUESTS **/

    $.fn.enterKey = function (fnc) {
        return this.each(function () {
            $(this).keypress(function (ev) {
                var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                if (keycode == '13') {
                    fnc.call(this, ev);
                }
            })
        })
    };

    var sendMessage = function() {

        var name = $("#name").val();

        if(name == "") {
            createAlert('Please type your name!', 'alert-danger');
        } else {
            var field = $("#field");
            socket.emit('send', { message:  field.val(), username: name });
            field.val("");
        }
    };

    $("#field").enterKey(function () { sendMessage(); });

    $("#send").click(function() { sendMessage();  });

    $("#join-game_type_1vs1").click( function() {
        socket.emit('join-game', { "game-type" : "game1vs1"});
    });

    $("#create-game_type_1vs1").click( function() {
        socket.emit('create-game', { "game-type" : "game1vs1"});
    });

    $("#request-game-data").click( function() {
        sendRequestGameData();
    });

    var sendRequestGameData = function() {
        socket.emit('request', { message: 'game-data'});
    };

    $("#spawn-ship").click( function() {
        socket.emit('spawn-ship', { message: 'spawn-ship'});
    });

    $("#start-game").click( function() {
        socket.emit('event', { "message" : "start-game" });
    });

    $("#pause-game").click( function() {
        socket.emit('event', { "message" : "pause-game" });
    });

    $("#resume-game").click( function() {
        socket.emit('event', { "message" : "resume-game" });
    });

    $("#end-game").click( function() {
        socket.emit('event', { "messaget" : "end-game" });
    });

    $("#leave-game").click( function() {
        socket.emit('leave-game', { "message" : "leave-game"});
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