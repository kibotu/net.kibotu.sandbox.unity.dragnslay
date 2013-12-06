using System;
using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    public abstract class Game : MonoBehaviour, IStringMessageEvent, IJSONMessageEvent
    {

        public float startTime;
        public static long turn;
        public Queue<Action> ExecuteOnMainThread;

        public void Start()
        {
            ExecuteOnMainThread = new Queue<Action>();
            startTime = 0;
            turn = 0;
        }

        public virtual void Update()
        {

            while (ExecuteOnMainThread.Count > 0)
            {
                ExecuteOnMainThread.Dequeue().Invoke();
            }


            // accept player commands

            // has turn-time elapsed?
            // no   -> analyze game & ping speed
            // yes  ->  'done' message & timing & count
            // increment 'command turn'
            // 'done' message for all players? 
            // no   -> process drop & timeout checks
            // yes  -> advance turn counter
            // adjust timing for new turn
            // do game turn (render, etc.)


            // handle game loop
            startTime += Time.deltaTime;
            if (startTime >= 1f)
            {
                startTime -= 1f;
                ++turn;

                //Debug.Log("game loop " + turn);
            }
        }

        internal static long ScheduleId()
        {
            return turn + 2;
        }

        public abstract void OnStringEvent(string message);
        public abstract void OnJSONEvent(JObject message);
    }
}
