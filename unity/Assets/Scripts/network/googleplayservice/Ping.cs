using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.network.googleplayservice;
using Newtonsoft.Json.Linq;
using Assets.Sources.network;

public class Ping : MonoBehaviour {

	public Text PingLabel; 
	public float PingTime;

	public void Start()
	{
		GooglePlayServiceHelper.Shared.RtsHandler.RealTimeMessageReceived += OnJSONEvent;
	}

	public void SendPing() 
	{
		GooglePlayServiceHelper.Shared.BroadcastMessage (PackageFactory.CreatePing());
	}

	public void SendPong() 
	{
		GooglePlayServiceHelper.Shared.BroadcastMessage (PackageFactory.CreatePong());
	}
	
	public void OnJSONEvent(JObject json, string senderId, bool isReliable)
	{
		var message = json["message"].ToString();
		
		if (message.Equals("ping"))
		{
			PingTime = Time.time;
			GooglePlayServiceHelper.Shared.BroadcastMessage (PackageFactory.CreatePong());
		}

		if (message.Equals("pong"))
		{
			PingLabel.text = Time.time - PingTime + "ms";
			PingTime = 0;
			GooglePlayServiceHelper.Shared.BroadcastMessage (PackageFactory.CreatePing());
		}
	}
}
