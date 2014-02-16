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
        public WorldData World;

        protected static GameState _gameState;

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
    }
}
