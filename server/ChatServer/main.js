process.env.NODE_ENV = 'development';

var express = require('express');
var socketHandler = require('./sockethandler');
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

var processLevelTemplate = function(levelData) {

    // islands and ship unique ids
    while(levelData.contains('%uid%')) {
        levelData = levelData.replace('%uid%', uid());
    }

    // player unique ids
    while(levelData.contains('%playeruid%')) {
        levelData = levelData.replace('%playeruid%', '"' + hat() + '"');
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
 /*
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

    getJsonObject("http://www.telize.com/geoip", uploadIps);
});
    */
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
            var res = JSON.parse(body)
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
server.configure('development', function(){
    server.set('network_interface', process.argv[2] ? process.argv[2] : 'Ethernet');  // LAN: 'Ethernet', WLAN: 'Wi-Fi', WAN: 'ip'
    server.set('ip_dirty', process.argv[3] ? process.argv[3] : false);
    server.set('tcp_port', 1337);
    server.set('udp_port', 1338);
    server.set('views', __dirname + '/views');
    server.set('view engine', 'jade');
    server.use(express.errorHandler());
    server.locals.pretty = true;
    server.use(express.favicon());
    server.use(express.logger('dev'));
    server.use(express.cookieParser('keyboard unicorn'));
    server.use(express.urlencoded());
    server.use(express.json());
    server.use(express.methodOverride());
    server.use(express.session({secret: 'keyboard unicorn', key: 'express.sid'}));
    // server.use(function (req, res) { res.end('<h2>Hello, your session id is ' + req.sessionID + '</h2>');  });
    server.use(server.router);
    server.use(express.static(path.join(__dirname, 'public')));
});

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
    console.log("info: tcp server listening on "+getIpAddress()[server.get('network_interface')]+":"+server.get('tcp_port'));
}));
var udp_socket = dgram.createSocket('udp4');
udp_socket.bind(server.get('udp_port'), function() {
    console.log("info: udp server listening on "+getIpAddress()[server.get('network_interface')]+":"+server.get('udp_port'));

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

//io.disable('heartbeats');
io.configure( function() {
    io.set('close timeout', 60 * 15); // 15 min
    io.set('log level', 3);
    io.set('reconnect', true);
    io.set('reconnection delay', 500);
    io.set('max reconnection attempts', 10);
    io.enable('browser client minification');  // send minified client
    //io.enable('browser client etag');          // apply etag caching logic based on version number
    //io.enable('browser client gzip');          // gzip the file
    io.set('transports', [
          'websocket'
        , 'flashsocket'
        , 'htmlfile'
        , 'xhr-polling'
        , 'jsonp-polling'
    ]);
    io.set('authorization', function (data, accept) {
        accept(null, true);
        console.log("Authorization : " + JSON.stringify(data));
    });
});

var loadGameData = function(game_type, callback) {

    if(game_type == 'game1vs1')
        fs.readFile( './../resources/levels/'+ game_type + '.json', function (err, data) {
            if(err)
                console.log(err);

            callback(JSON.parse(processLevelTemplate(decoder.write(data))));
        });

    else
        callback({ "error" : "game_type " + game_type + "not implemented!" });
};

var sendAll = function(socket, message) {
    socket.emit('message', message);
    socket.broadcast.to(socket.room).emit('message', message);
};

var sendAllTextMessage = function(socket, messageSocket, messageChannel){
    //console.log("send all message")
    socket.emit('message', { message: messageSocket });
    socket.broadcast.to(socket.room).emit('message', {message : messageChannel});
};

var sendAllInRoom = function(socket, messageSocket, messageChannel) {
    socket.emit('message', messageSocket);
    socket.broadcast.to(socket.room).emit('message', messageChannel);
};

// usernames which are currently connected to the chat
var users = {};

// rooms which are currently available in chat
var rooms = {'Queue' : []};

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

    rooms[socket.room].push(socket.uid);

    socket.join(socket.room);

    users[socket.uid] = socket;  // todo remove when disconnect
};

var leaveRoom = function(socket) {
    sendAllTextMessage(socket, "You left " + socket.room, socket.uid + " left " + socket.room);
    if(!socket || !socket.room) return;  // todo server might have zombie rooms
    rooms[socket.room].splice(socket.uid, 1);
    socket.leave(socket.room);

    // remove room, if empty
    if(rooms[socket.room].length < 1 && socket.room != 'Queue')
        delete rooms[socket.room];
};

var createRoom = function() {
    var roomId = hat();
    console.log("Create room " + roomId);
    rooms[roomId] = [];
    return roomId;
};

var changeRoom = function(socket, roomId) {

    if(socket.room == roomId)
        return;

    console.log(""+socket.uid + " changed from " + socket.room + " to " + roomId);
    sendAllTextMessage(socket, ""+ socket.uid + " changed from " + socket.room + " to " + roomId);
    leaveRoom(socket);
    joinRoom(socket,roomId);
};

var findAvailableRoomId = function(game_type)
{
    var availableRoom;

    _.map(rooms, function(num, key){

       // todo find available games by type and mmr
       // if(key != 'Queue' && rooms[key]['game-type'] == game_type) {
            //availableRoom = key;
       // }

        if(key != 'Queue') {
            availableRoom = key;
        }
    });

    return availableRoom ? availableRoom : createRoom();
};

// never fired~
io.sockets.on('connect', function (socket){
    var address = socket.handshake.address;
    console.log("New connection from " + address.address + ":" + address.port);
});

io.sockets.on('connection', function (socket) {

    // new client has connected
    var address = socket.handshake.address;
    console.log("New connection from " + address.address + ":" + address.port);

    // handshake unique id, which persists over multiple connections
    // 1) tell client its uid
    socket.tmpUid = hat();
    socket.game = {};
    socket.emit('message', { message: 'Welcome!', uid: socket.tmpUid });

    // echo
    socket.on('send', function (data) {
        sendAllInRoom(socket, data, data);
        console.log("Response:", data);
    });

    // ping
    socket.on('ping', function (data) {
        socket.emit('pong', data);
        console.log("ping? => pong");
    });

    // message
    socket.on("message", function(data) {

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
        console.log("message " + JSON.stringify(data));
        sendAllTextMessage(socket, data);

        updateRooms();
    });

    // create game
    socket.on('create-game', function(data){
        console.log("create game:", data);
        socket.game.game_type = data['game-type'];
        changeRoom(socket, createRoom());

        updateRooms();
    });

    // join game
    socket.on('join-game', function(data){
        console.log("join game:", data);
        socket.game.game_type = data['game-type'];
        changeRoom(socket, findAvailableRoomId(socket.game.game_type));

        updateRooms();
    });

    // join game
    socket.on('leave-game', function(data){
        console.log("leave game:", data);
        changeRoom(socket, 'Queue');

        updateRooms();
    });

    socket.on('request', function(data){

        console.log("client request:", data['message']);

        if(data['message'] == "game-data") {

            if(!socket.game.game_type) {
                sendAll(socket, {error : "Can't receive game-data yet. Join or create a game first!"});
                return;
            }

            console.log("sending game data for " + socket.game.game_type) ;
            loadGameData(socket.game.game_type, function(data){
                sendAll(socket, { "message" : "game-data", "game-data" : data } ); // todo send only to channel
            });
        }
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