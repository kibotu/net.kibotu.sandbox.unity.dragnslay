using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.components.data;
using Assets.Sources.model;
using Assets.Sources.network;
using Assets.Sources.states;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.game
{
    public abstract class GameMp : Game
    {
        public int CurrentTurn;
        public float TurnFrequency;
        public int ScheduledTotal;
        public int ScheduledToDo;
        public int ScheduledDone;
        public int Turn;
        public bool LoggingEnabled = false;

        public readonly static Dictionary<long, Queue<Action>> ExecuteOnMainThreadScheduled = new Dictionary<long, Queue<Action>>();

        public virtual void Start()
        {
            // 1) creating game
            GameState = GameState.Creating;
            StartTime = 0;
            Turn = 0;
        }

        /**
         * @see http://www.gamasutra.com/features/20010322/terrano_02.jpg
         * and regarding http://docs.unity3d.com/Documentation/Manual/ExecutionOrder.html
         */
        public override void Update()
        {
            base.Update();

            // stats for inspector
            CurrentTurn = (int) Turn;
            ScheduledToDo = ExecuteOnMainThreadScheduled.Count;

            // 3) is running?
            if (GameState != GameState.Running)
                return;

            // accept player commands

            // has turn-time elapsed?
            if (!hasTurnTimeElapsed())
            {
                // no   -> analyze game & ping speed
                analyzeGameAndPingSpeed();
            }
            else
            {
                // yes  ->  'done' message & timing & count
                DoneMessage();
                Timing();
                Count();
                // increment 'command turn'
                incrementCommandTurn();

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

        private void checkTimeOut()
        {
            // what happens if someone misses a 'turn-done' message?
        }

        private void processDrop()
        {

        }

        private bool DoneMessageOfAllPlayer()
        {
            var hasTurnDone = false;
            foreach (var playerData in Registry.Player.Values.Select(player => player.GetComponent<PlayerData>()).Where(playerData => playerData.playerType != PlayerData.PlayerType.Neutral))
            {
                hasTurnDone = playerData.Turn == Turn;
            }

            return hasTurnDone;
        }

        private void incrementCommandTurn()
        {

        }

        private void Count()
        {

        }

        private void Timing()
        {
            StartTime -= TurnFrequency;
        }

        private bool _hasSendTurnDoneMessage = false;

        private void DoneMessage()
        {
            if(_hasSendTurnDoneMessage) 
                return;

            Registry.Player[ClientUid].GetComponent<PlayerData>().Turn = Turn;

            SocketHandler.EmitNow("turn-done", PackageFactory.CreateDoneMessage(Turn));

            _hasSendTurnDoneMessage = true;
        }

        private void analyzeGameAndPingSpeed()
        {

        }

        private bool hasTurnTimeElapsed()
        {
            StartTime += Time.deltaTime;
            return StartTime > TurnFrequency;
        }

        private void adjustTimingForNewTurn()
        {
            
        }

        /// <summary>
        /// Schedules an action to a specific turn. Adds action to a queue, if there are multiple events at the same turn scheduled.
        /// </summary>
        /// <param name="turn">Turn number.</param>
        /// <param name="action">Action to be executed.</param>
        public void ScheduleAt(long turn, Action action)
        {
            ++ScheduledTotal;

            // see if already an action is scheduld at turn, if so add to queue, if not, create new queue
            Queue<Action> queue;
            ExecuteOnMainThreadScheduled.TryGetValue(turn, out queue);
            if (queue == null)
            {
                queue = new Queue<Action>();
                queue.Enqueue(action);
                ExecuteOnMainThreadScheduled.Add(turn, queue);
            }
            else
            {
                queue.Enqueue(action);
            }
        }

        protected virtual void DoGameTurn()
        {
            // execute all scheduled tasks for this turn
            Queue<Action> actions;
            ExecuteOnMainThreadScheduled.TryGetValue(Turn, out actions);
            if (actions != null)
            {
                // dequeue
                while (actions.Count > 0)
                {
                    actions.Dequeue().Invoke();
                    if (LoggingEnabled) Debug.Log("Execute scheduled action at turn: " + Turn);
                    ++ScheduledDone;
                }
                // remove actions when done
                ExecuteOnMainThreadScheduled.Remove(Turn);
            }
            _hasSendTurnDoneMessage = false;
        }

        private void AdvanceTurnCounter()
        {
            ++Turn;
        }

        public static long ScheduleId()
        {
            return ((GameMp)Shared).Turn + 2;
        }

        public bool IsHost()
        {
            return HostUid == ClientUid;
        }

        public abstract void OnJSONEvent(JObject message);
    }
}
