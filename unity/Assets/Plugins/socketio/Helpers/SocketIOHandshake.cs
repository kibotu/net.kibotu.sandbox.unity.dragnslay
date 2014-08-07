using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

namespace SocketIOClient
{
    public class SocketIOHandshake
    {
        public string SID { get; set; }
        public int HeartbeatTimeout { get; set; }
		public string ErrorMessage { get; set; }
		public bool HadError
		{
			get { return !string.IsNullOrEmpty(this.ErrorMessage); }

		}
		/// <summary>
		/// The HearbeatInterval will be approxamately 20% faster than the Socket.IO service indicated was required
		/// </summary>
        public TimeSpan HeartbeatInterval
        {
            get
            {
                return new TimeSpan(0, 0, HeartbeatTimeout);
            }
        }
        public int ConnectionTimeout { get; set; }
        public List<string> Transports = new List<string>();

		public SocketIOHandshake()
		{

		}

        public static SocketIOHandshake LoadFromString(string value)
        {
            var handshake = new SocketIOHandshake();
            if (string.IsNullOrEmpty(value)) return null;

            // socket.io 1.0.6: e.g. 97:0{"sid":"_IowAnL1F3DjC3sRAAAF","upgrades":["websocket"],"pingInterval":25000,"pingTimeout":60000}

            const string pattern = @":|{(.*)}";
            var tmp = Regex.Split(value, pattern);

            var msgLength = tmp[0];
            var msgType = tmp[2]; // tmp[1] is an empty space
            var msgJson = value.Substring(value.IndexOf("{", StringComparison.Ordinal));

//            Debug.Log("Parsed Response: " + msgLength + " " + msgType + " " + msgJson);

            var json = (Hashtable)MiniJSON.jsonDecode(msgJson);

            // outdated socket.io 0.9
//                string[] items = value.Split(new char[] { ':' });
//                if (items.Count() == 4)
//                {
//                    int hb = 0;
//                    int ct = 0;
//                    returnItem.SID = items[0];
//
//                    if (int.TryParse(items[1], out hb))
//                    { 
//                        var pct = (int)(hb * .75);  // setup client time to occure 25% faster than needed
//                        returnItem.HeartbeatTimeout = pct;
//                    }
//                    if (int.TryParse(items[2], out ct))
//                        returnItem.ConnectionTimeout = ct;
//                    returnItem.Transports.AddRange(items[3].Split(new char[] { ',' }));
//                    return returnItem;
//                }

            handshake.SID = (string)json["sid"];
            handshake.HeartbeatTimeout = (int)((double)json["pingInterval"] * .75); // my favorite expression; minijson knows only double numbers // setup client time to occure 25% faster than needed
            handshake.ConnectionTimeout = (int)(double)json["pingTimeout"];

//            Debug.Log(handshake.SID);
//            Debug.Log(handshake.HeartbeatTimeout);
//            Debug.Log(handshake.ConnectionTimeout);

            foreach (var transport in (ArrayList)json["upgrades"])
            {
                handshake.Transports.Add((string) transport);
            }

            return handshake;
        }
    }
}
