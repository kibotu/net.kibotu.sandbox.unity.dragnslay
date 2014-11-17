using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameLifecycle : MonoBehaviour 
{
	public int Turn;
	public float TurnFrequency;
	public int ScheduledTotal;
	public int ScheduledTodo;
	public int ScheduledDone;
	public int MainThreadQueue;
	public int ExecutedOnMainThreadDone;

	void Start () {
	
	}
	
	void Update () {
		
		MainThreadQueue = Extensions.ExecuteOnMainThread.Count;
		
		// dispatch stuff on main thread
		while (Extensions.ExecuteOnMainThread.Count > 0)
		{
			Extensions.ExecuteOnMainThread.Dequeue().Invoke();
			++ExecutedOnMainThreadDone;
		}
	}
}
