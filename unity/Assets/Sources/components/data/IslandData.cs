using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.data
{
    public class IslandData : MonoBehaviour
    {
        public void Awake() {

            if(Game.IsSinglePlayer())
            {
                uid = UidGenerator.GetNewUid();
                Registry.Islands.Add(uid, gameObject);

                maxSpawn = 1;
            }
        }

        public int uid;
        public int shipType;
        public int islandType;
        public string playerUid;
        public int maxSpawn;

        public float ShipBuildTime()
        {
            return 3f;
        }
    }
}
