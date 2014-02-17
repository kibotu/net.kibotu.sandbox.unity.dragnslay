using Assets.Sources.components.data;
using Assets.Sources.states;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.game
{
    public abstract class GameMp : Game
    {
        public static string HostUid;
        public float TurnFrequency;
        public static long Turn;

        public void Start()
        {
            // 1) creating game
            _gameState = GameState.Creating;
            World = gameObject.AddComponent<WorldData>();
            StartTime = 0;
            Turn = 0;
            TurnFrequency = 1000;
        }

        /**
         * @see http://www.gamasutra.com/features/20010322/terrano_02.jpg
         * and regarding http://docs.unity3d.com/Documentation/Manual/ExecutionOrder.html
         */
        public override void Update()
        {
            base.Update();

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

        public bool IsHost()
        {
            return HostUid == ClientUid;
        }

        public abstract void OnJSONEvent(JObject message);
    }
}
