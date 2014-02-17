using Assets.Sources.components.behaviours.combat;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.network;
using Assets.Sources.utility;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SpawnUnits : MonoBehaviour
    {
        public int MaxSpawn
        {
            get { return _islandData.maxSpawn; }
        }
        private float _startTime;
        private IslandData _islandData;
        private int _initAmountChildren;

        public float StartTime
        {
            get { return _islandData.ShipBuildTime() - _startTime; }
        }

        public void Start()
        {
            _startTime = 0;
            _islandData = GetComponent<IslandData>();
            _initAmountChildren = transform.childCount;
        }

        public void FixedUpdate ()
        {
            if (!Game.IsRunning()) return;

            _startTime += Time.deltaTime;
            if (_startTime < _islandData.ShipBuildTime()) return;
            _startTime -= _islandData.ShipBuildTime();

            // ship can already have other stuff
            if (transform.childCount >= _initAmountChildren + MaxSpawn) return;

            if (Game.IsSinglePlayer())
            {
                SpawnUnit();
            }
            else
            {
                SocketHandler.Emit("spawn-unit", PackageFactory.CreateSpawnMessage(
                    new[] { new JObject
                    {
                        {"island_uid", gameObject.GetComponent<IslandData>().uid},
                        {"uid" , -1}
                    }}));
            }
        }

        public void SpawnUnit()
        {
            var shipUid = UidGenerator.GetNewUid();
            var island = Registry.Islands[gameObject.GetComponent<IslandData>().uid];
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
            //go.GetComponentInChildren<Renderer>().material.color = island.renderer.material.color;

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
