using Newtonsoft.Json.Linq;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    interface IJSONMessageEvent
    {
        void OnJSONEvent(JObject message);
    }
}
