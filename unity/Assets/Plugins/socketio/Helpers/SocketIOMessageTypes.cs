using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient
{
    /// <summary>
    /// @see https://github.com/automattic/socket.io-protocol#packet
    /// 
    /// Packet#CONNECT (0)
    /// Packet#DISCONNECT (1)
    /// Packet#EVENT (2)
    /// Packet#ACK (3)
    /// Packet#ERROR (4)
    /// Packet#BINARY_EVENT (5)
    /// Packet#BINARY_ACK (6)
    /// </summary>
    public enum SocketIOMessageTypes
    {
        Disconnect = 1, //Signals disconnection. If no endpoint is specified, disconnects the entire socket.
        Connect = 0,    // Only used for multiple sockets. Signals a connection to the endpoint. Once the server receives it, it's echoed back to the client.
//        Heartbeat = 2,
//        Message = 3, // A regular message
//        JSONMessage = 4, // A JSON message
        Event = 2, // An event is like a JSON message, but has mandatory name and args fields.
        ACK = 3,  //An acknowledgment contains the message id as the message data. If a + sign follows the message id, it's treated as an event message packet.
        Error = 4, // Error
//        Noop = 8 // No operation


        
    

    }
}
