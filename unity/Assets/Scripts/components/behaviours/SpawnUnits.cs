using Assets.Sources.components.behaviours.combat;
using Assets.Sources.components.data;
using Assets.Sources.game;
using Assets.Sources.model;
using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components.behaviours
{
    public class SpawnUnits : MonoBehaviour
    {
        public float _startTime;
        public IslandData Island;

        public virtual void Start()
        {
            _startTime = 0;

            if(Game.IsSinglePlayer())
                Island = GetComponent<IslandData>();
        }

        private bool HasReachedMaxSpawn()
        {
            return Game.Shared.World.MaxPopulationLimitReached() || IslandData.AmountFriendlyUnits(Island) >= Island.MaxSpawn;
        }

        public virtual void FixedUpdate()
        {
            // 1) check against polulation limits
            if (HasReachedMaxSpawn())
            {
                _startTime = 0;
                return;
            }

            // 2) check against spawn time
            _startTime += Time.deltaTime;
            if (_startTime < Island.ShipBuildTime()) return;

            // 3) trigger spawn
            Spawn();
        }

        public virtual void Spawn()
        {
            var shipUid = UidGenerator.GetNewUid();
            var island = Registry.Islands[gameObject.GetComponent<IslandData>().Uid];
            var islandData = island.GetComponent<IslandData>();

            // 1) create ship by type
            var go = GameObjectFactory.CreateShip(shipUid, islandData.ShipType, islandData.PlayerData.playerType);

            // 2) append ship at island
            go.transform.Translate(island.transform.position);
            go.transform.parent = island.transform;

            // 3) set ship data
            var shipData = go.AddComponent<ShipData>();
            shipData.shipType = islandData.ShipType;
            shipData.uid = shipUid;
            shipData.PlayerData = islandData.PlayerData;

            // 4) colorize @see http://answers.unity3d.com/questions/483419/changing-color-of-children-of-instantiated-prefab.html
            //go.GetComponentInChildren<Renderer>().material.color = Island.PlayerData.color - new Color(0.5f,0.5f,0.5f);

            // 5) life data
            var lifeData = go.AddComponent<LifeData>();
            lifeData.CurrentHp = lifeData.MaxHp = 10;

            // 6) host fires attacks
            go.AddComponent<Assault>();
            go.AddComponent<Defence>();

            // 7) reset spawn timer
            ResetSpawnTimer();

            // Debug.Log("spawn [uid=" + shipUid + "|type=" + shipData.shipType + "] at [uid=" + islandData.uid + "|type=" + islandData.islandType  + "] for player [" + shipData.playerUid + "]");
        }

        public virtual void ResetSpawnTimer()
        {
            //Debug.Log("ShipBuildTime: " + Island.ShipBuildTime());
            _startTime -= Island.ShipBuildTime();
        }
    }
}
