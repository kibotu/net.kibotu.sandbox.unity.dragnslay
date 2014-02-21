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
        private float _startTime;
        public IslandData Island;
        private int _initAmountChildren;

        public float StartTime
        {
            get { return Island.ShipBuildTime() - _startTime; }
        }

        public void Start()
        {
            _startTime = 0;
            Island = GetComponent<IslandData>();
            _initAmountChildren = transform.childCount;
        }

        public void FixedUpdate ()
        {
            if (!Game.IsRunning()) return;

            _startTime += Time.deltaTime;
            if (_startTime < Island.ShipBuildTime()) return;
            _startTime -= Island.ShipBuildTime();

            // ship can already have other stuff
            if (transform.childCount >= _initAmountChildren + Island.maxSpawn) return;

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
            go.GetComponentInChildren<Renderer>().material.color = Island.PlayerData.color - new Color(0.5f,0.5f,0.5f);

            // 5) life data
            var lifeData = go.AddComponent<LifeData>();
            lifeData.CurrentHp = lifeData.MaxHp = 10;

            // 6) host fires attacks
            go.AddComponent<Assault>();
            go.AddComponent<Defence>();

            // Debug.Log("spawn [uid=" + shipUid + "|type=" + shipData.shipType + "] at [uid=" + islandData.uid + "|type=" + islandData.islandType  + "] for player [" + shipData.playerUid + "]");
        }
    }
}
