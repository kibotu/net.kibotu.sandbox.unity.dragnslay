using System.Linq;
using HutongGames.PlayMaker.Actions;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.network.googleplayservice
{
    public class Ping : MonoBehaviour {

        public Text PingLabel; 
        public float PingTime;

        public void Start()
        {
            GooglePlayServiceHelper.Shared.RtsHandler.RealTimeMessageReceived += OnJSONEvent;
        }

        public void SendPing()
        {
            PingTime = Time.time;
            GooglePlayServiceHelper.Shared.RtsHandler.BroadcastMessage (PackageFactory.CreatePing());
        }

        private static void ackTest()
        {
            Debug.Log("count: " + PackageDameon.Unverified.Count());

            var pingJson = PackageFactory.CreatePing();
            PackageDameon.Unverified.Add(pingJson);
            Debug.Log("count: " + PackageDameon.Unverified.Count());

            Debug.Log("ping: " + pingJson);

            var ack = PackageFactory.CreateReceivedMessage(pingJson["packageId"].ToObject<int>(),
                pingJson["scheduleId"].ToObject<int>());

            Debug.Log("ack: " + ack);

            var startJson = PackageDameon.Unverified.Acknowledge(ack);

            Debug.Log("startJson: " + startJson);

            Debug.Log("equals? " + startJson.Equals(pingJson));
            Debug.Log("count: " + PackageDameon.Unverified.Count());
        }

        public void SendPong()
        {
            GooglePlayServiceHelper.Shared.RtsHandler.BroadcastMessage(PackageFactory.CreatePong());
        }

        public void OnJSONEvent(JObject json, string senderId)
        {
            var message = json["message"].ToString();
		
            if (message.Equals("ping"))
            {
                PingTime = Time.time;
                GooglePlayServiceHelper.Shared.RtsHandler.BroadcastMessage(PackageFactory.CreatePong());
            }

            if (message.Equals("pong"))
            {
                PingLabel.text = Time.time - PingTime + "ms";
                PingTime = Time.time;
                GooglePlayServiceHelper.Shared.RtsHandler.BroadcastMessage(PackageFactory.CreatePing());
            }
        }
    }
}
