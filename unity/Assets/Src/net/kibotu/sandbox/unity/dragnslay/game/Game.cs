using System;
using System.Collections.Generic;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.States;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    public abstract class Game : MonoBehaviour, IJSONMessageEvent
    {

        public float StartTime;
        public float TurnFrequency;
        public static long Turn;
        public static Queue<Action> ExecuteOnMainThread;
        public WorldData World;
        public static string ClientUid;
        public static string HostUid;

        private static GameState _gameState;

        public void Start()
        {
            // 1) creating game
            _gameState = GameState.Creating;

            ExecuteOnMainThread = new Queue<Action>();
            World = gameObject.AddComponent<WorldData>();
            StartTime = 0;
            Turn = 0;
            TurnFrequency = 1000;
        }

        public void StartGame()
        {
            // 2) start game
            _gameState = GameState.Running;
        }

        public void PauseGame()
        {
            _gameState = GameState.Pause;
        }

        public void ResumeGame()
        {
            _gameState = GameState.Running;
        }

        public void StopGame()
        {
            _gameState = GameState.Stopped;
        }
        
        public static bool IsRunning()
        {
            return _gameState == GameState.Running;
        }

        /**
         * @see http://www.gamasutra.com/features/20010322/terrano_02.jpg
         */
        public virtual void Update()
        {
            // dispatch stuff on main thread
            while (ExecuteOnMainThread.Count > 0)
            {
                ExecuteOnMainThread.Dequeue().Invoke();
            }

            // 3) is running?
            if (_gameState != GameState.Running)
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
                doneMessage();
                Timing();
                Count();
                // increment 'command turn'
                incrementCommandTurn();

                // 'done' message for all players? 
                if (!doneMessageOfAllPlayer())
                {
                    // no   -> process drop & timeout checks
                    processDrop();
                    checkTimeOut();
                }
                else
                {
                    // yes  -> advance turn counter
                    advanceTurnCounter();
                    // adjust timing for new turn
                    adjustTimingForNewTurn();
                    // do game turn (render, etc.)
                    DoGameTurn();
                }
            }
                       
        }

        private void checkTimeOut()
        {
            
        }

        private void processDrop()
        {
            
        }

        private bool doneMessageOfAllPlayer()
        {
            return true;
        }

        private void incrementCommandTurn()
        {
            
        }

        private void Count()
        {
            
        }

        private void Timing()
        {
            
        }

        private void doneMessage()
        {
            
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
            StartTime -= TurnFrequency; 
        }

        protected abstract void DoGameTurn();

        private void advanceTurnCounter()
        {
            ++Turn;
        }

        internal static long ScheduleId()
        {
            return Turn + 2;
        }

        public abstract void OnJSONEvent(JObject message);
    }
}
