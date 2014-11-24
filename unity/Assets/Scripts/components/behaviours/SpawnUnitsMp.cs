using System.Runtime.InteropServices;
using Assets.Scripts.network;
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
    public class SpawnUnitsMp : SpawnUnits
    {
        private PlayMakerFSM _fsm;

        public override void Start()
        {
            base.Start();
            _fsm = GetComponent<PlayMakerFSM>();
        }

        public override void Spawn()
        {
            // check if spawning event already triggered
            if (_fsm.ActiveStateName.Equals("Spawning Ship"))
                return;

            // 1) block spawnings until network event did actually spawn the ship
            _fsm.SendEvent("Spawn");

            // 2) broadcast spawn event
            var json = PackageFactory.CreateSpawnMessage(
                    new[] { new JObject
                        {
                            {"island_uid", Island.Uid},
                            {"uid" , UidGenerator.GetNewUid()}
                        }});
            SocketHandler.EmitNow("spawn-unit", json);

            // 3 schedule event
            Spawn(json);
        }

        public void Spawn(JObject json)
        {
            foreach (var spawn in json["spawns"])
            {
                var ship = (JObject)spawn;
                Debug.Log("spawn units scheduled at: " + json["scheduleId"]);

                ((GameMp) Game.Shared).ScheduleAt("spawn-unit", json["scheduleId"].ToObject<long>(),
                    json["packageId"].ToObject<int>(), () =>
                    {
                        var shipUid = ship["uid"].ToObject<int>();
                        var island = Registry.Islands[ship["island_uid"].ToObject<int>()];
                        var islandData = island.GetComponent<IslandData>();

                        // 1) create ship by type
                        var go = GameObjectFactory.CreateShip(shipUid, islandData.ShipType,
                            islandData.PlayerData.playerType);

                        // 2) append ship at island
                        go.transform.Translate(island.transform.position);
                        go.transform.parent = island.transform;
                        go.transform.localScale = new Vector3(1f, 1f, 1f);

                        // 3) set ship data
                        var shipData = go.AddComponent<ShipData>();
                        shipData.shipType = islandData.ShipType;
                        shipData.uid = shipUid;
                        shipData.PlayerData = islandData.PlayerData;

                        // 4) colorize @see http://answers.unity3d.com/questions/483419/changing-color-of-children-of-instantiated-prefab.html
                        //go.GetComponentInChildren<Renderer>().material.color = island.renderer.material.color;

                        // 5) life data
                        var lifeData = go.AddComponent<LifeData>();
                        lifeData.CurrentHp = lifeData.MaxHp = 10;

                        // 6) host fires attacks
                        if (GameMp.IsHost())
                        {
                            go.AddComponent<Assault>();
                            shipData.AttackDamage = 2;
                            shipData.AttackSpeed = 1000;
                            go.AddComponent<Defence>();
                        }

                        // 7) re-enable spawning
                        island.GetComponentInChildren<SpawnUnitsMp>().ResetSpawnTimer();

                        // Debug.Log("spawn [uid=" + shipUid + "|type=" + shipData.shipType + "] at [uid=" + islandData.Uid + "|type=" + islandData.IslandType + "] for player [" + shipData.PlayerData.uid + "]");
                    });
            }
        }

        public override void ResetSpawnTimer()
        {
            if (!GameMp.IsHost())
                return;

            base.ResetSpawnTimer();
            _fsm.SendEvent("Spawned");
        }
    }
}