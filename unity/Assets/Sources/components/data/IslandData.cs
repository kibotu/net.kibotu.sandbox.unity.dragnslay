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

            // debug singleplayer
            if (uid == 0)
            {
                uid = UidGenerator.GetNewUid();
                Registry.Islands.Add(uid, gameObject);
            }

            PlayerData = Registry.Player[PlayerData.uid].GetComponent<PlayerData>();

			Dye ();
        }

        public int uid;
        public int shipType;
        public int islandType;

        public void Convert(PlayerData PlayerData)
        {
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
