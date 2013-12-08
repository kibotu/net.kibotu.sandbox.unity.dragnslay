
var spawn = require('child_process').spawn;
var events = require('events');

// Configure event emitter
var eventEmitter= new events.EventEmitter();

// Buffering is required to avoid partial lines being emitted
var buffer = { stdout: [], strderr: [] };

module.exports = function(cmd, name, options, interval){

    setInterval(function() {

        // Spawn netstat daemon
        var daemon = spawn( cmd, options, { encoding: 'binary' } );

        daemon.stdout.on( 'data', function( data ) {
            data = buffer.stdout + data.toString( 'utf-8' );
            buffer.stdout = data.substring( data.lastIndexOf('\n') );
            eventEmitter.emit( name,data.substring( 0, data.lastIndexOf('\n') ));
        });

        daemon.stderr.on( 'data', function( data ) {
            data = buffer.stderr + data.toString( 'utf-8' );
            buffer.stderr = data.substring( data.lastIndexOf('\n') );
            eventEmitter.emit( 'stderr', data.substring( 0, data.lastIndexOf('\n') ) );
        });

        daemon.on('exit', function () {
            daemon.kill();
            // console.log("exitting child process");
        });

    }, interval);

    return eventEmitter;
};