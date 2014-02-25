using System;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.data
{
    public class IslandData : MonoBehaviour
    {
        public void Start() {

            if(Game.IsSinglePlayer())
            {
                uid = UidGenerator.GetNewUid();
                Registry.Islands.Add(uid, gameObject);

                maxSpawn = 5;
            }

            PlayerData = Registry.Player[playerUid].GetComponent<PlayerData>();
        }

        public int uid;
        public int shipType;
        public int islandType;

        [Obsolete("Not used anymore use PlayerData instead", false)]
        public string playerUid;

        public void Convert(PlayerData PlayerData)
        {
            playerUid = PlayerData.uid;
            this.PlayerData = PlayerData;
            Dye();
            var shockwave = Prefabs.Instance.GetNewShockwave();
            shockwave.transform.position = transform.position;
            shockwave.GetComponent<DetonatorShockwave>().color = PlayerData.color;
        }

        public int maxSpawn;

        public PlayerData PlayerData;

        public float ShipBuildTime()
        {
            return 3f;
        }

        public void Dye()
        {
            renderer.material.color = PlayerData.color;
        }
    }
}
