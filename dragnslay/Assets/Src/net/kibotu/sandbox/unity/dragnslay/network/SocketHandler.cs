using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using SimpleJson;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    class SocketHandler : MonoBehaviour
    {
         #if UNITY_ANDROID && !UNITY_EDITOR
        private AndroidJavaClass _socket;
         #endif
        private static SocketHandler _instance;
        private Queue<MessageData> messageQueue;
        private string host;
        private int port;

        public void Awake()
        {
            messageQueue = new Queue<MessageData>();

           // serverIp = "http://192.168.198.50:3000"; 
            host = "178.0.89.213";
            port = 1337;

#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJNIHelper.debug = true;
            if (_socket == null)
            {
                _socket = new AndroidJavaClass("net.kibotu.sandbox.chat.client.android.network.SocketClient");
                _socket.CallStatic("connect", host, port);
            }
#endif
        }

        public static SocketHandler Instance
        {
            get { return _instance ?? (_instance = new GameObject("SocketHandler").AddComponent<SocketHandler>()); }
        }

        /// <summary>
        /// Emitting a message to the server.
        /// </summary>
        /// <param name="name">MessageData name.</param>
        /// <param name="message">Json message.</param>
        public void Emit(string name, JsonObject message)
        {
            Emit(name, message.ToString());
        }

        public void Emit(string name, string message)
        {
            var msg = new MessageData {name = name, message = message};
            Debug.Log("Enqueue " + msg.name + " " + msg.message);
            messageQueue.Enqueue(msg);
        }

        /// <summary>Sending Units from all sources to target destination.</summary>
        /// 
        /// <param name="ships">All send ships.</param>
        /// <param name="target">Target destination.</param>
        /// 
        /// <returns>Generated JsonObject.</returns>
        public JsonObject CreateSendUnitsMessage(int target, int[] ships)
        {
            return new JsonObject{
                {"message", "move-units"},
                {"ships", ships},
                {"target", target}
            };
        }

        public JsonObject CreateHelloWorldMessage()
        {
            return new JsonObject
            {
                {"message", "hallo welt"},
                {"username", "android"},
                {"name", "message"},
            };
        }

        public void StringCallback(string message)
        {
            Debug.Log("StringCallback " + message);
        }

        public void JSONCallback(string message)
        {
            Debug.Log("JSONCallback " + message);
        }

        public void DisconnectCallback(string error)
        {
            Debug.Log("DisconnectCallback " + error);
        }

        public void ErrorCallback(string error)
        {
            Debug.Log("ErrorCallback " + error);
        }

        public void ReconnectCallback(string message)
        {
            // message always null
            Debug.Log("ReconnectCallback");
        }

        public void ConnectionFailedCallback(string message)
        {
            Debug.Log("ConnectionFailedCallback" + message);
        }

        public void Update()
        {
            while (messageQueue.Count > 0)
            {
                var msg = messageQueue.Dequeue();
                #if UNITY_ANDROID && !UNITY_EDITOR
                _socket.CallStatic("Emit", msg.name, msg.message);
                #endif
            }
        }
    }
}
