using System;
using Assets.Sources.components.behaviours.combat;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.behaviours.singleplayer
{
    public class SpawnUnitsSp : MonoBehaviour
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
            
            SpawnUnit();
        }

        public void SpawnUnit()
        {
            var shipUid = UidPool.GetNewUid();
            var island = Registry.Instance.Islands[gameObject.GetComponent<IslandData>().uid];
            var islandData = island.GetComponent<IslandData>();

            // 1) create ship by type
            var go = GameObjectFactory.CreateShip(shipUid, islandData.shipType);

            // 2) append ship at island
            go.transform.Translate(island.transform.position);
            go.transform.parent = island.transform;

            // 3) set ship data
            var shipData = go.AddComponent<ShipData>();
            shipData.shipType = islandData.shipType;
            shipData.uid = shipUid;
            shipData.playerUid = islandData.playerUid;

            // 4) colorize @see http://answers.unity3d.com/questions/483419/changing-color-of-children-of-instantiated-prefab.html
            go.GetComponentInChildren<Renderer>().material.color = island.renderer.material.color;

            // 5) life data
            var lifeData = go.AddComponent<LifeData>();
            lifeData.CurrentHp = lifeData.MaxHp = 10;

            // 6) host fires attacks
            var attack = go.AddComponent<Assault>();
            attack.AttackDamage = 2;
            attack.AttackSpeed = 1000;
            go.AddComponent<Defence>();

            Debug.Log("spawn [uid=" + shipUid + "|type=" + shipData.shipType + "] at [uid=" + islandData.uid + "|type=" + islandData.islandType  + "] for player [" + shipData.playerUid + "]");

        }
    }
}
