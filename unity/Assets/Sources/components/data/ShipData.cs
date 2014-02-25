using System;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.components.data
{
    public class ShipData : MonoBehaviour
    {
        public int uid;
        public int shipType;
        [Obsolete("Not used anymore use PlayerData instead", false)]
        public string playerUid;
        public float AttackSpeed;
        public float AttackDamage;

        public PlayerData PlayerData;

        public void Start()
        {
            AttackSpeed = Random.Range(1f,2f);
            AttackDamage = Random.Range(1.7f,2.5f);
            if (Game.IsSinglePlayer())
            {
                uid = UidGenerator.GetNewUid();
            }

            PlayerData = Registry.Player[playerUid].GetComponent<PlayerData>();
            GetComponentInChildren<Renderer>().material.color = PlayerData.color;
        }
    }
}
