using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data
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
