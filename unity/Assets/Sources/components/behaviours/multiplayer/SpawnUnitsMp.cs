using System;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.components.behaviours.multiplayer
{
    public class SpawnUnitsMp : MonoBehaviour
    {
        public int MaxSpawn;
        private float _startTime;
        private IslandData _islandData;
        private int _initChildren;
        public Action SpawnUnity;

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
