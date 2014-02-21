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
            renderer.material.color = PlayerData.color;
        }

        public int uid;
        public int shipType;
        public int islandType;
        public string playerUid;
        public int maxSpawn;

        public PlayerData PlayerData;

        public float ShipBuildTime()
        {
            return 3f;
        }
    }
}
