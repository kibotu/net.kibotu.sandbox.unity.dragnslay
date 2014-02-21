using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.data
{
    public class ShipData : MonoBehaviour
    {
        public int uid;
        public int shipType;
        public string playerUid;
        public float AttackSpeed = 1;
        public float AttackDamage = 2;

        public PlayerData PlayerData;

        public void Start()
        {
            if (Game.IsSinglePlayer())
            {
                uid = UidGenerator.GetNewUid();
            }

            PlayerData = Registry.Player[playerUid].GetComponent<PlayerData>();
            GetComponentInChildren<Renderer>().material.color = PlayerData.color;
        }
    }
}
