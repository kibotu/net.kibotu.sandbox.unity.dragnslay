using SimpleJson;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    class SocketHandler : MonoBehaviour
    {
        private AndroidJavaClass _socket;
        private static SocketHandler _instance;

        public static SocketHandler Instance
        {
            get { return _instance ?? (_instance = new GameObject("SocketHandler").AddComponent<SocketHandler>()); }
        }

        public void Emit(string name, JsonObject message)
        {
            Emit(name, message.ToString());
        }

        public void Emit(string name, string message)
        {
            #if UNITY_ANDROID
            if (_socket == null)
            {
                AndroidJNIHelper.debug = true;
                _socket = new AndroidJavaClass("net.kibotu.sandbox.unity.android.SocketHandler");
                _socket.CallStatic("connect", "http://172.19.253.37:3000");
            }
            _socket.CallStatic("Emit", name, message);
            #endif
        }

        public JsonObject createSendUnitsMessage(int [] source, int dest, int [] ships)
        {
            return new JsonObject{
                {"name", "move-units"},
                {"source", source},
                {"dest", dest},
                {"amount", ships} 
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
    }
}
