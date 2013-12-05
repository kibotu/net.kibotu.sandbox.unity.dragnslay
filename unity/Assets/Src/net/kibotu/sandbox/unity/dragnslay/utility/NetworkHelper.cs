using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;


// @see asynchronity http://www.dannygoodayle.com/2013/06/11/asynchronous-synchronous-unity-beginners/
// @see coroutines http://unitygems.com/coroutines/
// @see http://blog.sebaslab.com/run-serial-parallel-asynchronous-tasks-unity3d-c/
// @see taskqueue https://github.com/dkozar/eDriven/blob/master/eDriven.Core/Tasks/TaskQueue.cs
namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.utility
{
    internal class NetworkHelper 
    {

        // static
        private NetworkHelper()
        {
        }

        private delegate String DownloadFileDelegate(WWW www);
        private delegate Dictionary<string, object> DownloadJsonDelegate(WWW www);

        public static void DownloadFileByUrl(string url, AsyncCallback callback)
        {
            // 1) invoke new delegate with async callback
            new DownloadFileDelegate(DownloadFile).BeginInvoke(new WWW(url), callback, null);
        }

        public static void DownloadJsonByUrl(string url, AsyncCallback callback)
        {
            // 1) invoke new delegate with async callback
            new DownloadJsonDelegate(DownloadJson).BeginInvoke(new WWW(url), callback, null);
        }
       
        private static string DownloadFile(WWW www) 
        {
            // 2) download file
            Console.WriteLine("DownloadFile() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            return www.text;
        }

        private static Dictionary<string, object> DownloadJson(WWW www)
        {
            // 2) download file
            Console.WriteLine("DownloadJson() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            return DeserializeToDictionary(www.text);
        }

        public static Dictionary<string, object> DeserializeToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json).ToDictionary(d => d.Key, d => d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject") ? DeserializeToDictionary(d.Value.ToString()) : d.Value);
        }
    }
}
