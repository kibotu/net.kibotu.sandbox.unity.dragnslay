using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    class JsonHelper
    {
        // static
        private JsonHelper()
        {
        }

        public static Dictionary<string, object> DeserializeToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json).ToDictionary(d => d.Key, d => d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject") ? DeserializeToDictionary(d.Value.ToString()) : d.Value);
        }
    }
}
