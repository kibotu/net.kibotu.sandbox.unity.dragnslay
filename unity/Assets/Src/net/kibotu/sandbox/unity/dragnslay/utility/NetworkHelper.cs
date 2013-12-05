using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;


// @see asynchronity http://www.dannygoodayle.com/2013/06/11/asynchronous-synchronous-unity-beginners/
// @see coroutines http://unitygems.com/coroutines/
// @see http://blog.sebaslab.com/run-serial-parallel-asynchronous-tasks-unity3d-c/
// @see taskqueue https://github.com/dkozar/eDriven/blob/master/eDriven.Core/Tasks/TaskQueue.cs
namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    internal class NetworkHelper : MonoBehaviour
    {
        public void Awake()
        {
        }

        public void Start()
        {
        }

        public void RequestIpAddress(String url, AsyncCallback callback)
        {
            StartCoroutine(Download(url, callback)); 
        }

        private IEnumerator Download(string url, AsyncCallback callback)
        {
            var www = new WWW(url);
            yield return www;
            var jsonObject = DeserializeToDictionary(www.text);
            Debug.Log(jsonObject[(string) jsonObject["network_interface"]]);
        }

        private Dictionary<string, object> DeserializeToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json).ToDictionary(d => d.Key, d => d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject") ? DeserializeToDictionary(d.Value.ToString()) : d.Value);
        }
    }
}
