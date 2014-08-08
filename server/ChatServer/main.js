process.env.NODE_ENV = 'development';
process.stdout.setEncoding('utf8');
process.stderr.setEncoding('utf8');

// https://www.npmjs.org/package/memwatch
// http://simonmcmanus.wordpress.com/2013/01/03/forcing-garbage-collection-with-node-js-and-v8/
setInterval(function() {
    global.gc();
},1000);

// nice to know: http://codetunnel.com/blog/post/9-javascript-tips-you-may-not-know#null-undefined-and-delete
var nodetime = require('nodetime').profile({
    accountKey: '95c1612507ed5224a07691cfdb47436e9467c146',
    appName: 'Drag\'n Slay'
});

// http://stackoverflow.com/a/1590262
var MainQueue = { queue : [] };

var Enqueue = function(callback) {
    if(!callback || typeof(callback) !== "function") {
        console.trace("Failed to enqueue a non-function! (" + (callback ? typeof(callback) : "null") + " " + traceback()[0].name + " at line /main.js:" + traceback()[1].line + ")");
        return;
    }
    MainQueue.queue.push(callback);
};

/*
setInterval(function() {

    while(MainQueue.queue.length > 0) {
        MainQueue.queue.shift()();
    }

},16);*/

var express = require('express');
var socketHandler = require('./src/network/sockethandler');
var routes = require('./routes');
var user = require('./routes/user');
var chat = require('./routes/chat');
var admin = require('./routes/admin');
var http = require('http');
var path = require('path');
var hat = require('hat');
var _ = require('underscore');
var dgram = require('dgram');  // http://nodejs.org/api/dgram.html
var StringDecoder = require('string_decoder').StringDecoder;
var decoder = new StringDecoder('utf8');
var os = require('os');
var dns = require('dns');
var jsftp = require("jsftp");
var fs = require('fs');
var request = require('request');
var Seq = require('seq');
var ftp;
var uid = require('./src/utils/uid');
var Game = require('./src/game/game');
var osutils = require('os-utils');
var traceback = require('traceback');

var getUids = function(number) {
    var res = new Number[number];
    for(var i = 0; i < number; ++i) {
        res[i] = uid();
    }
    return res;
};

// network traffic: @see http://stackoverflow.com/a/18907653
var startNetstat = function() {

    var netstat = require('./src/network/shelldameon')( 'c:/windows/system32/netstat', 'netstat', [ '-es' ],10000);

    netstat.on( 'netstat', function( data ){
        var lines = data.split('\n');
        // console.log(data);
    });

    netstat.on( 'stderr', function( err ) {
        process.stderr.write( err );
    });
};

var startTasklist = function() {

    var takslist = require('./src/network/shelldameon')( 'c:/windows/system32/tasklist', 'tasklist', ['/fi', 'IMAGENAME eq node.exe'], 30000);  // '/fi "IMAGENAME eq node.exe"'

    takslist.on( 'tasklist', function( data ){
        var nodeps = _.findWhere(data.split('\n'), "node");
        if(nodeps) {
            var nodemem = nodeps.split(" ");
            console.log(nodemem[nodemem.length-2]);
        }
    });

    takslist.on( 'stderr', function( err ) {
        process.stderr.write( err );
    });
};      startTasklist();

// @see http://de.slideshare.net/bluesmoon/a-nodejs-bag-of-goodies-for-analyzing-web-traffic
// var geo = require('geip-lite');
// var loc = geo.lookup(ip);
// var qs = require('querystring');
// qs.parse('name=larry&name=moe&name=curly');
// {name : ['larry','moe','curly']}
// var gauss = require('gauss');
// var set new gauss.Vector(5,1,3,2,21);
// console.log(set.mean());

var getOsInfos = function(callback) {

    var osinfos = {
        platform: osutils.platform(),
        cpuCount: osutils.cpuCount(),
        //sysUptime: osutils.sysUptime(),// already in monitor
        processUptime: osutils.processUptime(),
        // freemem: osutils.freemem(),  // already in monitor
       // totalmem: osutils.totalmem(), // already in monitor
        freememPercentage: osutils.freememPercentage(),
        allLoadavg: osutils.allLoadavg()
        //loadavg: osutils.loadavg() // already in monitor
    };

    osutils.cpuUsage(function(usage){
        osinfos.cpuUsage = usage;
        osutils.cpuFree(function(cpuFree){
            osinfos.cpuFree = cpuFree;
//            osutils.freeCommand(function(freeCommand){  // only linux
//                osinfos.freeCommand = freeCommand;
//                osutils.harddrive(function(harddrive){  // only linux
//                    osinfos.harddrive = harddrive;
                    osutils.getProcesses(function(processes){
                        osinfos.getProcesses = processes;
                        callback(osinfos);
                    });
//                });
//            });
        });
    });
};

var osm = require("os-monitor");
var osStats;

var startOsMonitor = function() {

    // basic usage
    osm.start();

    // more advanced usage with configs.
    osm.start({ delay: 333 // interval in ms between monitor cycles
        , freemem: 1000000000 // amount of memory in bytes under which event 'freemem' is triggered (can also be a percentage of total mem)
        , uptime: 1000000 // number of seconds over which event 'uptime' is triggered
        , critical1: 0.7 // value of 1 minute load average over which event 'loadavg1' is triggered
        , critical5: 0.7 // value of 5 minutes load average over which event 'loadavg5' is triggered
        , critical15: 0.7 // value of 15 minutes load average over which event 'loadavg15' is triggered
        , silent: false // set true to mute event 'monitor'
        , stream: false // set true to enable the monitor as a Readable Stream
    });

    // define handler that will always fire every cycle
    osm.on('monitor', function(event) {
        getOsInfos(function(json){
            _.extend(event, json);
            osStats = event;

            //osStats.clients = io.sockets.clients().length;
            //if(io.sockets.clients().length > 0) {
                //console.log("\n");
                 //  console.log(io.sockets.manager.server);
//                console.log(io.sockets.manager.connected);
//                console.log(io.sockets.manager.open);
//                console.log(io.sockets.manager.closed);
//                console.log(io.sockets.manager.handshaken);
           // }
        });
    });

    // define handler for a too high 1-minute load average
    osm.on('loadavg1', function(event) {
        console.log(event.type, ' Load average is exceptionally high!');
    });

    // define handler for a too low free memory
    osm.on('freemem', function(event) {
       // console.log(event.type, 'Free memory is very low!');
    });

    // define a throttled handler, using Underscore.js's throttle function (http://underscorejs.org/#throttle)
    osm.throttle('loadavg5', function(event) {

        // whatever is done here will not happen
        // more than once every 5 minutes(300000 ms)

    }, osm.minutes(5));

    // change config while monitor is running
    osm.config({
        freemem: 0.3 // alarm when 30% or less free memory available
    });

    // stop monitor
    //osm.stop();

    // check either monitor is running or not
    //osm.isRunning(); // -> true / false
};

startOsMonitor();

var g1 = new Game("bla");
console.log(g1.player);

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

String.prototype.contains = function(it) { return this.indexOf(it) != -1; };

var processLevelTemplate = function(levelData, player, bots) {

    // islands and ship unique ids
    while(levelData.contains('%uid%')) {
        levelData = levelData.replace('%uid%', uid());
    }

    // player unique ids
    var i = 0;
    while(levelData.contains('%playeruid%')) {
        levelData = levelData.replace('%playeruid%', '"' + player[i++] + '"');
    }

    i = 0;
    while(levelData.contains('%botuid%')) {
        levelData = levelData.replace('%botuid%', '"' + bots[i++] + '"');
    }

    return levelData;
};

// udp http://stackoverflow.com/questions/9545153/transfer-udp-socket-in-node-js-from-application-to-http

var server = express();

var getIpAddress = function() {
    var ifaces = os.networkInterfaces();
    var ips = {};
    for (var dev in ifaces) {
        var alias=0;
        ifaces[dev].forEach(function(details){
            if (details.family=='IPv4') {
                ips[dev+(alias?':'+alias:'')] = details.address;
                ++alias;
            }
        });
    }
    ips['network_interface'] = server.get('network_interface');
    ips['tcp_port'] = server.get('tcp_port');
    ips['udp_port'] = server.get('udp_port');
    _.extend(ips, server.get('server-geo-infos'));
    return ips;
};

var getIpAddressByDnsLookup = function(callback) {
    ip = dns.lookup(require('os').hostname(), function (err, add, fam) {
        callback(add);
    });
};

var php_json_encode = function(json) {
  return json.replace('/g', '\/');
};

var updateServerJson = function(callback) {

    fs.readFile( 'pwd', function (err, data) {
        if (err)
            throw err;

        var pwd = JSON.parse(decoder.write(data));

        ftp = new jsftp({
            host: pwd.host,
            port: pwd.port,     // defaults to 21
            user: pwd.user,     // defaults to "anonymous"
            pass: pwd.pass      // defaults to "@anonymous"
        });

        getJsonObject("http://www.telize.com/geoip", callback);
    });
};

var uploadIps = function(geoJson) {

    server.set("server-geo-infos", geoJson);

    // update ip if it has changed
    if(!server.get('ip_dirty')) return;

    // upload current ip address as json for clients to get
    uploadFileByFtp(getIpAsJsonResponsePhp(), "/server.php", function() {
        // upload current ip address for server admin panel
        uploadFileByFtp(getServerAdminUrlPhp(), "/admin.php", function() {
        });
    });
};

var getIpAsJsonResponsePhp = function() {
    return "<?php header('Content-Type: application/json; charset=utf8'); header('Access-Control-Allow-Origin: *'); header('Access-Control-Max-Age: 3628800'); header('Access-Control-Allow-Methods: GET"+/*, POST, PUT, DELETE*/"'); echo '" + php_json_encode(JSON.stringify(getIpAddress())) + "'; ?>";
};

var getServerAdminUrlPhp = function() {
    return "<?php header('HTTP/1.1 301 Moved Permanently'); header(\"Expires: Mon, 26 Jul 12012 05:00:00 GMT\"); header(\"Last-Modified: \" . gmdate(\"D, d M Y H:i:s\") . \" GMT\"); header(\"Cache-Control: no-store, no-cache, must-revalidate\"); header(\"Cache-Control: post-check=0, pre-check=0\", false); header(\"Pragma: no-cache\"); header('Location: http://"+getIpAddress()[server.get('network_interface')]+":"+server.get('tcp_port')+"/admin'); exit(); ?>";
};

var uploadFileByFtp = function(filedata, targetUrl, callback) {
    var buffer = new Buffer(filedata, "utf-8");
    ftp.on('progress', function(progress) {
        console.log("progress:",progress);
    });
    ftp.put(buffer, targetUrl, function(err) {
        if(err)
            console.log(err);

        console.log("File successfully uploaded: " + targetUrl, decoder.write(buffer, "utf-8"));

        if(callback) callback();
    });
};

var listFilesFpt = function(url) {
    ftp.list(url, function(err, res) {
        try {
            res.forEach(function(file) {
                console.log(file.name);
            });
        } catch(error) {
            console.log(error);
        }
    });
};

var getJsonObject = function(url, callback) {
    http.get(url, function(res) {
        var body = '';
        res.on('data', function(chunk) {
            body += chunk;
        });

        res.on('end', function() {
            var res = JSON.parse(body);
            callback(res)
        });
    }).on('error', function(e) {
            console.log("Got error: ", e);
    });
};

// print process.argv
process.argv.forEach(function (val, index, array) {
    console.log(index + ': ' + val);
});

// development
server.set('network_interface', process.argv[2] ? process.argv[2] : 'Ethernet');  // LAN: 'Ethernet', WLAN: 'Wi-Fi', WAN: 'ip'
server.set('ip_dirty', process.argv[3] ? process.argv[3] : false);
server.set('tcp_port', 1337);
server.set('udp_port', 1338);
server.set('views', __dirname + '/views');
server.set('view engine', 'jade');
//server.use(express.errorHandler());
server.locals.pretty = true;
//server.use(express.favicon());
//server.use(express.logger('dev'));
//server.use(express.cookieParser('keyboard unicorn'));
//server.use(express.urlencoded());
//server.use(express.json());
//server.use(express.methodOverride());
//server.use(express.session({secret: 'keyboard unicorn', key: 'express.sid'}));
// server.use(function (req, res) { res.end('<h2>Hello, your session id is ' + req.sessionID + '</h2>');  });
//server.use(server.router);
server.use(express.static(path.join(__dirname, 'public')));
server.use(express.static(__dirname + '/components'));

server.get('/', routes.index);
server.get('/users', user.list);
server.get('/chat', chat.list);
server.get('/admin', admin.list);

var sendUdpMessage = function(string, rinfo) {
    var message = new Buffer(string);
    udp_socket.send(message, 0, message.length, rinfo.port, rinfo.address, function(err, bytes) {
        if(err)
            console.log(err);
        if(bytes)
            console.log(bytes + ' bytes => ' + rinfo.address + ":"+  rinfo.port + "\t=>\t" + decoder.write(string));
    });
};

// socket io on udp
var io = require('socket.io').listen(server.listen(server.get('tcp_port'),"0.0.0.0", function() {

    updateServerJson(function(geoJson) {
        uploadIps(geoJson);
        console.log("info: tcp server listening on "+getIpAddress()[server.get('network_interface')]+":"+server.get('tcp_port'));
        console.log("info: udp server listening on "+getIpAddress()[server.get('network_interface')]+":"+server.get('udp_port'));
    });
}));

//var io = require('socket.io').listen(engine.listen(server.get('tcp_port'),"0.0.0.0", function() {
//
//    updateServerJson(function(geoJson) {
//        uploadIps(geoJson);
//        console.log("info: tcp server listening on "+getIpAddress()[server.get('network_interface')]+":"+server.get('tcp_port'));
//        console.log("info: udp server listening on "+getIpAddress()[server.get('network_interface')]+":"+server.get('udp_port'));
//    });
//}));

var udp_socket = dgram.createSocket('udp4');
udp_socket.bind(server.get('udp_port'), function() {

    udp_socket.on('message', function(data, rinfo) {

        if(decoder.write(data) == "ping") {
            sendUdpMessage('pong', rinfo);
        } else {
            sendUdpMessage(data, rinfo);
        }
    });

    udp_socket.on('listening', function(data, rinfo) {
        console.log('listening', data, JSON.stringify(data), 'from', rinfo.address, rinfo.port);
    });

    udp_socket.on('close', function(data, rinfo) {
        console.log('close', data, JSON.stringify(data), 'from', rinfo.address, rinfo.port);
    });

    udp_socket.on('error', function (data, rinfo) {
        console.log('error', data, JSON.stringify(data), 'from', rinfo.address, rinfo.port);
    });
});

// @see https://github.com/LearnBoost/Socket.IO/wiki/Configuring-Socket.IO
//    io.set('close timeout', 60 * 15); // 15 min
//    io.set('heartbeat timeout', 60 * 2);
//    io.set('heartbeat interval', 60);
//    io.set('log level', 5);
//    io.set('reconnect', true);
//    io.set('reconnection delay', 500);
//    io.set('max reconnection attempts', 10);
//    io.enable('browser client minification');  // send minified client
    //io.enable('browser client etag');          // apply etag caching logic based on version number
    //io.enable('browser client gzip');          // gzip the file
    io.set('transports',
        ['websocket',
        'flashsocket',
        'htmlfile',
        'xhr-polling',
        'jsonp-polling',
        'polling']);
    io.set('authorization', function (data, accept) {
        accept(null, true);
//        console.log("Authorization : " + JSON.stringify(data));
    });

// { roomId : game_data }
var cachedGames = {};

var loadGameData = function(roomId, callback) {

    var game_type = rooms[roomId].game.game_type;
    var player = rooms[roomId].user;
    var bots = rooms[roomId].bots;

    if(game_type == 'game1vs1')
        fs.readFile( './../resources/levels/'+ game_type + '.json', function (err, data) {
            if(err)
                console.log(err);

            // if exists, use cache
            if(cachedGames[roomId]) {
                callback(cachedGames[roomId]);
                return;
            }

            var levelData = JSON.parse(processLevelTemplate(decoder.write(data),player, bots));

            // add host uid
            levelData['host-uid'] = player[0];

            // bots are always ready
            var players = levelData.players;
            _.each(players, function(player) {
                _.each(bots, function(bot){
                    if(player.uid == bot) {
                        player['client-ready'] = true;
                    }
                });
            });

            // add to cache
            cachedGames[roomId] = levelData;

            callback(levelData);
        });

    else
        callback({ "error" : "game_type " + game_type + "not implemented!" });
};

var sendAll = function(socket, type, messageChannel, messageSocket) {
    socket.emit(type, messageChannel ? messageChannel : messageSocket);
    socket.broadcast.to(socket.room).emit(type, messageChannel);
};

var sendAllTextMessage = function(socket, messageChannel, messageSocket){
    socket.emit('message', { message: messageSocket ? messageSocket : messageChannel });
    socket.broadcast.to(socket.room).emit('message', {message : messageChannel});
};

var sendAllInRoom = function(socket, type, messageChannel, messageSocket) {
    socket.emit(type,messageChannel ? messageChannel : messageSocket);
    socket.broadcast.to(socket.room).emit(type,messageChannel);
};

var sendAllInRoomTextMessage = function(socket, messageChannel, messageSocket) {
    socket.emit('message',messageChannel ? messageChannel : messageSocket);
    socket.broadcast.to(socket.room).emit('message',messageChannel);
};

var sendTextMessageInRoomToEveryoneElse = function(socket, messageChannel) {
    socket.broadcast.to(socket.room).emit("message",messageChannel);
};

// usernames which are currently connected to the chat
var users = {};

// rooms which are currently available in chat
var rooms = {'Queue' : { user : [], game : { }}};

// http://psitsmike.com/2011/10/node-js-and-socket-io-multiroom-chat-tutorial/
var updateRooms = function() {
    _.each(users, function(socket) {
        socket.emit("update-room", rooms);
    });
};

var joinRoom = function(socket, roomId) {
    sendAllTextMessage(socket,  "You joined " + roomId, socket.uid + " joined " + roomId);
    socket.room = roomId;

    if(!rooms[socket.room])
        console.log("WARNING!!!! room lost in space");

    rooms[socket.room].user.push(socket.uid);

    socket.join(socket.room);

    users[socket.uid] = socket;
};

var leaveRoom = function(socket) {
    sendAllTextMessage(socket, "You left " + socket.room, socket.uid + " left " + socket.room);
    if(!socket || !socket.room) return;  // todo server might have zombie rooms
    rooms[socket.room].user.splice(socket.uid, 1);
    socket.leave(socket.room);

    // remove room, if empty
    if(_.isEmpty(rooms[socket.room].user) && socket.room != 'Queue')
        delete rooms[socket.room];
};

var createRoom = function(game_type) {
    var roomId = hat();
    console.log("Create room " + roomId);
    rooms[roomId] = { user : [], bots : [hat()], game : { game_type : game_type }};
    return roomId;
};

var changeRoom = function(socket, roomId) {

    if(socket.room == roomId)
        return;

    console.log(socket.uid + " changed from " + socket.room + " to " + roomId);
    sendAllTextMessage(socket, socket.uid + " changed from " + socket.room + " to " + roomId);
    leaveRoom(socket);
    joinRoom(socket,roomId);
};

var findAvailableRoomId = function(game_type)
{
    var availableRoom;

    _.map(rooms, function(value, key){

        // todo find available games by type and mmr
        // if(key != 'Queue' && rooms[key]['game-type'] == game_type) {
            //availableRoom = key;
        // }

//        console.log("rooms:");
//        console.log("key:",key);
//        console.log("num:",value);

        if(key != 'Queue' && !roomHasEnoughPlayer(key)) {
            availableRoom = key;
        }
    });

    return availableRoom ? availableRoom : createRoom(game_type);
};

var roomHasEnoughPlayer = function(roomId) {
    var room = rooms[roomId];
    var res = false;

    //console.log("\n" + JSON.stringify(room) + "\n");

    if(room.game.game_type == 'game1vs1') {
        res = room.user.length == 2;
    }

    if(room.game.game_type == 'game2vs2') {
        res = room.user.length == 4;
    }

    return res;
};

var allPlayersAreReady = function(socket) {

    var ready = true;

    // is everyone ready?
    _.each(rooms[socket.room].game.data.players, function(player){

        // one player not ready?
        if(!player['client-ready']) {
            sendAllInRoomTextMessage(socket,{ "message" : "waiting-for-player", "player" : [player.uid] });
            ready = false;
        }
    });

    return ready;
};

io.sockets.on('connect', function (socket){
    console.log("New connect from " + socket.request.connection.remoteAddress + ":" + socket.request.connection.remotePort);

});
var bla = false;
io.sockets.on('connection', function (socket) {

    // new client has connected
    console.log("New connection from " + socket.request.connection.remoteAddress + ":" + socket.request.connection.remotePort);

    socket.emit('message', { message: 'hello world'});

    // enable latency spam
    setInterval(function() {
        socket.startTime = Date.now();
        socket.emit('message', { message: 'ping'});
    }, 2000);

    // handshake unique id, which persists over multiple connections
    // 1) tell client its uid
    socket.tmpUid = hat();
    socket.emit('message', { message: 'Welcome!', uid: socket.tmpUid });

    // echo
    socket.on('send', function (data) {

        data = parseJson(data);

        sendAllInRoomTextMessage(socket, data, data);
        console.log("Response:", data);
    });

    // pong
    socket.on('pong', function (data) {
        var latency = Date.now() -  socket.startTime;
        socket.emit('message', { message: 'latency', latency: latency});
//        console.log("received pong" + " " + data);
    });

    // ping
    socket.on('ping', function (data) {

        data = parseJson(data);

        //socket.emit('pong', data);
        socket.emit('message', { message: 'pong'});
        console.log("ping? => pong");
    });

    // get os stats
    socket.on('os-stats', function(data) {
        getOsInfos(function(osStats) {
//            osStats.clients = io.sockets.adapter.rooms[rooms[0]].length; // TODO connected clients
            socket.emit('os-stats', osStats);
        });
    });

    // message
    socket.on("message", function(data) {

        console.log(data);

        data = parseJson(data);

        // 2) finish handshake
        if(data.uid) {

            if(data.uid != socket.tmpUid)
                console.log("Warning, socket send different uid! socketId = (" + socket.tmpUid + ") != sendId (" + data.uid + ")");

            // acknowledge uid
            if(!socket.uid) {
                socket.uid = data.uid;
                console.log("socket uid", data.uid);
            }

            // join default room
            joinRoom(socket, 'Queue');
        }

        // debug
        console.log("Default",JSON.stringify(data));
        //sendAllTextMessage(socket, data);

        updateRooms();
    });

    // create game
    socket.on('create-game', function(data){

        data = parseJson(data);

        console.log("create game:", data);

        // 1) change room from queue to new room
        // 1.1) room creates new game by type
        changeRoom(socket, createRoom(data['game-type']));

        // 2) emitting waiting for player message
        socket.emit('message',{"message" : "waiting-for-player", player : [] });

        // 3) updating rooms
        updateRooms();
    });

    // join game
    socket.on('join-game', function(data){

        data = parseJson(data);

        // 1) check if room available
        // 1.1) change room to available by game_type
        // 1.2) create new room by game_type
        changeRoom(socket, findAvailableRoomId(data['game-type']));

        // 2) check if enough player are in room
        if(roomHasEnoughPlayer(socket.room))
            // 2.1) if yes emitting game-ready message
            sendAllInRoomTextMessage(socket, {message : "server-game-ready"});
        else
            // 2.2) if not emitting waiting for player message
            socket.emit('message',{"message" : "waiting-for-player"});

        // 3) update rooms
        updateRooms();
    });

    socket.on('turn-done', function(data) {
        //socket.emit('message',parseJson(data));
        sendTextMessageInRoomToEveryoneElse(socket, parseJson(data));
    });

    socket.on('acknowledged', function(data) {
        sendTextMessageInRoomToEveryoneElse(socket, parseJson(data));
    });

    socket.on('client-game-ready', function(data) {

        console.log("Receiving client-game-ready");

        // current player is ready
        _.each(rooms[socket.room].game.data.players, function(player) {
            if(player.uid == socket.uid)
                player['client-ready'] = true;
        });

        // start game, when all clients are ready
        if(allPlayersAreReady(socket) && !rooms[socket.room].game['running']) {
            rooms[socket.room].game['running'] = true;
            sendAllInRoomTextMessage(socket, { "message" : "start-game"});
        }
    });

    // join game
    socket.on('leave-game', function(data){

        data = parseJson(data);

        console.log("leave game:", data);
        changeRoom(socket, 'Queue');

        updateRooms();
    });

    socket.on('request', function(data){

        data = parseJson(data);

        console.log("client request:", data['message']);

        if(data['message'] == "game-data") {

            // 1) check if room is 'server-game-ready' (enough player in room and game mode chosen)
            if(!rooms[socket.room].game.game_type) {
                sendAllTextMessage(socket, {error : "Can't receive game-data yet. Join or create a game first!"});
                return;
            }

            // 2) creating game
            // 2.1) loading game-data by game_type
            // 2.2) broadcast game-data to clients
            loadGameData(socket.room, function(data) {
                console.log("sending game data for " + rooms[socket.room].game.game_type);
                rooms[socket.room].game.data = data;

                socket.emit('message',{ "message" : "game-data", "game-data" : data });
                // sendAllInRoomTextMessage(socket, { "message" : "game-data", "game-data" : data } );
            });
        }

        if(data['message'] == "uids") {
            socket.emit("message", {"message" : "uids", "uid" : getUids(data['amount'])});
        }
    });

    socket.on('spawn-unit', function(data){

        data = parseJson(data);

        // 1) replace invalid uids with valid ones
        _.each(data['spawns'], function(spawn) {
            if(spawn.uid == -1) spawn.uid = uid();
        });

        sendTextMessageInRoomToEveryoneElse(socket, data);
    });

    socket.on("move-unit", function(data) {
        data = parseJson(data);
        sendTextMessageInRoomToEveryoneElse(socket, data);
    });

    socket.on("unit-arrival", function(data) {
        data = parseJson(data);
        sendTextMessageInRoomToEveryoneElse(socket, data);
    });

    // reconnects
    socket.on('reconnect', function(data){
        console.log("reconnect:", data);

        updateRooms();
    });

    // disconnects
    socket.on('disconnect', function(){
        console.log("disconnect: " + socket.uid + " from " + socket.room);
        if(!socket || !socket.room) return; // todo server might have zombie users
        leaveRoom(socket);
        updateRooms();
        socket.emit('message', { message: 'You have disconnected from ' + socket.room });
        socket.broadcast.to(socket.room.key).emit('message', { message : socket.uid + ' has disconnected.' });
        socket.leave(socket.room);

        delete users[socket.uid];
        delete socket.uid;  // todo test after reconnect uids might be off

        updateRooms();
    });

    // error handling
    socket.on('error', function (reason){
        console.error('Unable to connect Socket.IO ' +  reason);
        socket.destroy();

        updateRooms();
    });

    // uncaught errors
    socket.on('uncaughtException', function (err) {
        console.error('Caught exception: ' + err.stack);

        updateRooms();
    });
});

var parseJson = function(data) {
    return _.isString(data) ? JSON.parse(data) : data;
};

// # serving the flash policy file
net = require("net");

net.createServer(function(socket) {
    //just added
    socket.on("error", function() {
        console.log("Caught flash policy server socket error: ");
        console.log(err.stack);
    });

    socket.write('<?xml version="1.0"?>\n');
    socket.write('<!DOCTYPE cross-domain-policy SYSTEM "http://www.macromedia.com/xml/dtds/cross-domain-policy.dtd">\n');
    socket.write("<cross-domain-policy>\n");
    socket.write('<allow-access-from domain="*" to-ports="*"/>\n');
    socket.write('</cross-domain-policy>\n');
    socket.end();
}).listen(843);