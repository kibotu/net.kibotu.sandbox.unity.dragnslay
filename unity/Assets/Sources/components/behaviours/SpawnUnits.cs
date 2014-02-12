using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    public class SpawnUnits : MonoBehaviour
    {
        public int MaxSpawn;
        private float _startTime;
        private IslandData _islandData;
        private int _initChildren;

        public float StartTime
        {
            get { return _islandData.ShipBuildTime() - _startTime; }
        }

        public void Start()
        {
            _startTime = 0;
            _islandData = GetComponent<IslandData>();
            MaxSpawn = _islandData.maxSpawn;
            _initChildren = transform.childCount;
        }

        public void Update ()
        {
            if (!Game.IsRunning()) return;

            _startTime += Time.deltaTime;
            if (_startTime < _islandData.ShipBuildTime()) return;
            _startTime -= _islandData.ShipBuildTime();

            // 1st child - sphere collider for ship interception
            if (transform.childCount >= _initChildren + MaxSpawn) return;
                SocketHandler.Emit("spawn-unit", PackageFactory.CreateSpawnMessage(
                    new[] { new JObject
                    {
                        {"island_uid", gameObject.GetComponent<IslandData>().uid},
                        {"uid" , -1}
                    }}));
        }
    }
}
