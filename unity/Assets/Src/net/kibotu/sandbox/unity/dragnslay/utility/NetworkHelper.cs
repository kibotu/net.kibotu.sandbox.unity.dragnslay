using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;


// @see asynchronity http://www.dannygoodayle.com/2013/06/11/asynchronous-synchronous-unity-beginners/
// @see coroutines http://unitygems.com/coroutines/
// @see http://blog.sebaslab.com/run-serial-parallel-asynchronous-tasks-unity3d-c/
// @see taskqueue https://github.com/dkozar/eDriven/blob/master/eDriven.Core/Tasks/TaskQueue.cs
namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    public class NetworkHelper
    {
        public static void DownloadJson(string url, Action<Dictionary<string, object>> action)
        {
            Coroutiner.StartCoroutine(DownloadAsJson(url, action));
        }

        private static IEnumerator DownloadAsJson(string url, Action<Dictionary<string, object>> action)
        {
            var www = new WWW(url);
            yield return www;
            action.Invoke(JsonHelper.DeserializeToDictionary(www.text));
        }
    }
}
