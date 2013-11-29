using System;
using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using SimpleJson;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    class SocketHandler : MonoBehaviour
    {
        public event Action<String> OnConnectEvent;
        public event Action<String> OnJSONEvent;
        public event Action<String> OnStringEvent;
        public event Action<String> OnConnectionFailedEvent;
        public event Action<String> OnReconnectEvent;
        public event Action<String> OnErrorEvent;
        public event Action<String> OnDisconnectEvent;

        #if UNITY_ANDROID && !UNITY_EDITOR
        private AndroidJavaClass _socket;
        #endif
        private static SocketHandler _instance;
        private Queue<MessageData> messageQueue;

        public void Awake()
        {
            messageQueue = new Queue<MessageData>();
        }

        public void Connect(string host, int port)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJNIHelper.debug = true;
            if (_socket == null)
            {
                _socket = new AndroidJavaClass("net.kibotu.sandbox.chat.client.android.network.SocketClient");
                // System.Threading.Thread(_socket.CallStatic("connect", host, port));
                _socket.CallStatic("connect", host, port);
            }
            #endif
        }

        public void Connect(int port)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJNIHelper.debug = true;
            if (_socket == null)
            {
                _socket = new AndroidJavaClass("net.kibotu.sandbox.chat.client.android.network.SocketClient");
                _socket.CallStatic("connect", port);
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

        public void ConnectCallback(string error)
        {
            OnConnectEvent(error);
        }

        public void StringCallback(string message)
        {
            // remove pointless json array artefact [ ] 
            OnStringEvent(message.Substring(1, message.Length - 2));
        }

        public void JSONCallback(string message)
        {
            OnJSONEvent(message);
        }

        public void ReconnectCallback(string message)
        {
            OnReconnectEvent(message);
        }

        public void DisconnectCallback(string error)
        {
            OnDisconnectEvent(error);
        }

        public void ErrorCallback(string error)
        {
            OnErrorEvent(error);
        }

        public void ConnectionFailedCallback(string message)
        {
            OnConnectionFailedEvent(message);
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
