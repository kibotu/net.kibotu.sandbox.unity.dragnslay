using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SimpleJson;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.network
{
    interface IJSONMessageEvent
    {
        void OnJSONEvent(JObject message);
    }
}
