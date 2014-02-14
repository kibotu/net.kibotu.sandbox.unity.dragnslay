using Newtonsoft.Json.Linq;

namespace Assets.Sources.network
{
    interface IJSONMessageEvent
    {
        void OnJSONEvent(JObject message);
    }
}
