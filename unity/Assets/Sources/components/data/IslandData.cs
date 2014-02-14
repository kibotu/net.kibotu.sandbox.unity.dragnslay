using UnityEngine;

namespace Assets.Sources.components.data
{
    class IslandData : MonoBehaviour
    {
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
