using Assets.Sources.game;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.data
{
    public class PlayerData : MonoBehaviour
    {
        public string uid;
        public int fbId;
        public Color color;
        public int level;
        public int xp;
        public int currancy;
        public int hardCurrancy;
        public int games_played;
        public int games_won;
        public int games_lost;
        public int games_left;
        public int[] friendlist;
        public PlayerType playerType;

        public void Start()
        {
            if (Game.IsSinglePlayer())
            {
                uid = UidGenerator.GetNewUid().ToString();
            }
        }

        public enum PlayerType
        {
            Player, Offensive, Neutral
        }
    }
}