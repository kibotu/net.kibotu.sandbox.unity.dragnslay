using Assets.net.kibotu.sandbox.unity.dragnslay.pattern;
using SimpleJson;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    class SocketClient : MonoBehaviour
    {
        private AndroidJavaClass _socket;
        private static SocketClient _instance;

        public static SocketClient Instance
        {
            get { return _instance ?? (_instance = new GameObject("SocketClient").AddComponent<SocketClient>()); }
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
                _socket = new AndroidJavaClass("net.kibotu.sandbox.unity.android.SocketFacade");
                _socket.CallStatic("setUrl", "http://172.19.253.37:3000");
            }
            _socket.CallStatic("Emit", name, message);
            #endif
        }

        public JsonObject createSendUnitsMessage(int source, int dest, int [] ships)
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
    }
}
