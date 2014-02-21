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
        public readonly  static Queue<Action> ExecuteOnMainThread = new Queue<Action>();
        public static string ClientUid = "Client";
        public WorldData World;

        public enum Mode { SinglePlayer, Game1vs1, Game2vs2 }
        public static Mode GameMode = Mode.SinglePlayer;

        protected static GameState _gameState;

        public void Awake()
        {
            World = GetComponent<WorldData>();
        }

        public virtual void Update()
        {
            // dispatch stuff on main thread
            while (ExecuteOnMainThread.Count > 0)
            {
                ExecuteOnMainThread.Dequeue().Invoke();
            }

            // do stuff
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

        public static bool IsSinglePlayer()
        {
            return GameMode == Mode.SinglePlayer;
        }
    }
}
