using UnityEngine;
using System.Collections;
using Assets.Sources.components;
using UnityEngine.UI;
using Assets.Scripts.network.googleplayservice;
using Newtonsoft.Json.Linq;

public class GPSRTTest : MonoBehaviour {

	public Console Console;
	public bool VerticalSynchronization = true;

	// gps

	public Text Ping;
	public Text SendMessages;
	public Text ReceivedMessages;
	public Text SendBytes;
	public Text ReceivedBytes;

	// gps packages
	public Text LastPackageId;

	// game logic
	public GameLifecycle Game;
	public Text Turn;
	public Text TurnFrequency;
	public Text ScheduledTotal;
	public Text ScheduledTodo;
	public Text ScheduledDone;
	public Text ExecutedOnMainThread;

	public void Start() {
		//GooglePlayServiceHelper.Shared.BroadcastMessage ();

		GooglePlayServiceHelper.Shared.RtsHandler.RealTimeMessageReceived += onJson;
	}

	public void onJson(JObject json, string senderId, bool isReliable) {
		LastPackageId.text = "" + json ["packageId"].ToObject<int> ();
		var ack = json ["ack"].ToObject<bool> ();
	}

	public void ToggleConnectionType(Text field) {       
		GooglePlayServiceHelper.Shared.Type = GooglePlayServiceHelper.Shared.Type == GooglePlayServiceHelper.ConnectionType.TCP 
			? GooglePlayServiceHelper.ConnectionType.UDP 
			: GooglePlayServiceHelper.ConnectionType.TCP;
		field.text = GooglePlayServiceHelper.Shared.Type.ToString();
	}

	public void ToggleConsole() {
		Console.Show = !Console.Show;
	}

	public void SetFps(Slider slider) {
		Application.targetFrameRate  = (int)slider.value;
	}

	public void SetTimescale(Slider slider) {
		Time.timeScale  = slider.value;
	}



	void Update() {
		QualitySettings.vSyncCount = VerticalSynchronization ? 1 : 0;

		Turn.text = Game.Turn.ToString();
		TurnFrequency.text = Game.TurnFrequency.ToString();
		ScheduledTotal.text = Game.ScheduledTotal.ToString();
		ScheduledTotal.text = Game.ScheduledTotal.ToString();
		ScheduledTodo.text = Game.ScheduledTodo.ToString();
		ScheduledDone.text = Game.ScheduledDone.ToString();
		ExecutedOnMainThread.text = Game.ExecutedOnMainThreadDone.ToString();
	}
}
