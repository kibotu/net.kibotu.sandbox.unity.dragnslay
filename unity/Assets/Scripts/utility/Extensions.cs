using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static partial class Extensions {

	public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

	public static Action ScheduleOnMainThread(this Action action) {
		ExecuteOnMainThread.Enqueue (action);
		return action;
	}
}
