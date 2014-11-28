using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.network.googleplayservice;
using Assets.Scripts.network;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using Newtonsoft.Json.Linq;

public class GameLifecycle : MonoBehaviour 
{
	public static volatile int Turn;
	public float TurnFrequency;
	public int ScheduledTotal;
	public int ScheduledTodo;
	public int ScheduledDone;
	public int MainThreadQueue;
	public int ExecutedOnMainThreadDone;

	private float StartTime;
	private bool _hasSendTurnDoneMessage;
	private volatile ArrayList Schedule;
	private Dictionary<String, DragnSlayUser> players;

	void Start () {

		// default values
		StartTime = 0;
		Turn = 0;
		_hasSendTurnDoneMessage = false;
		Schedule =  new ArrayList();

		// register json listener
		GooglePlayServiceHelper.Shared.RtsHandler.RealTimeMessageReceived += onJson;

		// reference current player
		players = new Dictionary<string, DragnSlayUser> ();
		foreach (Participant participant in PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ()) {
			var player = new DragnSlayUser() {participant = participant, Turn = 0};
			players[participant.ParticipantId] = player;
		}
		Debug.Log (players.Count + " player connected during start.");
	}
	
	void Update () {
		
		MainThreadQueue = Extensions.ExecuteOnMainThread.Count;
		ScheduledTodo = Schedule.Count;
		
		// dispatch stuff on main thread
		while (Extensions.ExecuteOnMainThread.Count > 0)
		{
			Extensions.ExecuteOnMainThread.Dequeue().Invoke();
			++ExecutedOnMainThreadDone;
		}

		if (!hasTurnTimeElapsed())
		{
			// no   -> analyze game & ping speed
			analyzeGameAndPingSpeed();
		}
		else
		{
			// yes  ->  'done' message & timing & count
			SendDoneMessage();
			Timing();
			
			// 'done' message for all players? 
			if (!DoneMessageOfAllPlayer())
			{
				// no   -> process drop & timeout checks
				processDrop();
				checkTimeOut();
			}
			else
			{
				// yes  -> advance turn counter
				AdvanceTurnCounter();
				// adjust timing for new turn
				adjustTimingForNewTurn();
				// do game turn (render, etc.)
				DoGameTurn();
			}
		}
	}

	private bool hasTurnTimeElapsed()
	{
		StartTime += Time.deltaTime;
		return StartTime > TurnFrequency;
	}

	private void Timing ()
	{
		StartTime -= TurnFrequency;
	}

	private bool DoneMessageOfAllPlayer ()
	{
		foreach(var p in players.Values) {
			if(p.Turn < Turn) 
				return false;
		}

		return true;
	}
	
	private void analyzeGameAndPingSpeed ()
	{
		
	}
	
	private void checkTimeOut ()
	{
		
	}

	private void processDrop ()
	{

	}
	
	private void adjustTimingForNewTurn ()
	{
		
	}

	private void AdvanceTurnCounter ()
	{
		++Turn;
	}

	public static int ScheduleId()
	{
		return Turn + 2;
	}

	private void DoGameTurn ()
	{
		
	}

	private void SendDoneMessage ()
	{
		if(_hasSendTurnDoneMessage) 
			return;
		
		GooglePlayServiceHelper.Shared.RtsHandler.BroadcastMessage (PackageFactory.CreateDoneMessage(Turn));
		
		_hasSendTurnDoneMessage = true;
	}

	public void onJson(JObject json, string senderId) {

		var message = json["message"].ToString();
		if (message.Equals ("turn-done")) 
		{
			players[senderId].Turn = json["turn"].ToObject<int>();
			var ga = new GameAction();
			ga.ScheduledTurn = players[senderId].Turn + 2;
			ga.action = ()=> Debug.Log ("Sender " + senderId + " finished turn " + players[senderId].Turn);
			Schedule.Add(ga);
		}
	}
}
