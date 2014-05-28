using Assets.Sources.game;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.network
{
    public class Ping : MonoBehaviour
    {
        public void Start()
        {
            SocketHandler.SharedConnection.OnJSONEvent += OnJSONEvent;
        }

        public void OnJSONEvent(JObject json)
        {
            var message = json["message"].ToString();

            if (message.Equals("ping"))
            {
                SocketHandler.EmitNow("pong", PackageFactory.CreatePong());
            }

            if (message.Equals("latency"))
            {
                Game.ExecuteOnMainThread.Enqueue(() =>
                {
                    guiText.text = json["latency"] + " ms";
                });
            }
        }
    }
}
