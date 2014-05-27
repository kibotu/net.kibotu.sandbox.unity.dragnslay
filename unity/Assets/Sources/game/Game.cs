using System;
using System.Collections.Generic;
using Assets.Sources.components.data;
using Assets.Sources.states;
using UnityEngine;

namespace Assets.Sources.game
{
    public abstract class Game : MonoBehaviour
    {
        public float StartTime;
        public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();
        public string ClientUid = "Client";
        public string HostUid;
        public WorldData World;
        public float Timescale = 1f;
        public static Game Shared;
        public int ExecutedOnMainThreadDone;
        public int MainThreadQueue;

        public enum Mode { SinglePlayer, Game1vs1, Game2vs2 }
        public static Mode GameMode = Mode.SinglePlayer;

        protected static GameState GameState;

        public virtual void Awake()
        {
            Shared = this;
            World = GetComponent<WorldData>();
        }

        public virtual void Update()
        {
            Time.timeScale = Timescale;
            MainThreadQueue = ExecuteOnMainThread.Count;

            // dispatch stuff on main thread
            while (ExecuteOnMainThread.Count > 0)
            {
                ExecuteOnMainThread.Dequeue().Invoke();
                ++ExecutedOnMainThreadDone;
            }
        }

        public void StartGame()
        {
            // 2) start game
            GameState = GameState.Running;
        }

        public void PauseGame()
        {
            GameState = GameState.Pause;
        }

        public void ResumeGame()
        {
            GameState = GameState.Running;
        }

        public void StopGame()
        {
            GameState = GameState.Stopped;
        }

        public static bool IsRunning()
        {
            return GameState == GameState.Running;
        }

        public static bool IsSinglePlayer()
        {
            return GameMode == Mode.SinglePlayer;
        }
    }
}
