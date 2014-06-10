using System.Linq;
using System.Runtime.InteropServices;
using Assets.Sources.components.behaviours;
using Assets.Sources.components.behaviours.combat;
using Assets.Sources.components.data;
using Assets.Sources.model;
using Assets.Sources.network;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Sources.game
{
    public class Game1vs1 : GameMp {

        const float Scale = 20;

        public Game1vs1()
        {
            GameMode = Mode.Game1vs1;
        }

        public override void OnJSONEvent(JObject json)
        {
            base.OnJSONEvent(json);
//            Debug.Log("message : " + json);

            var message = json["message"].ToString();

            #region acknowledge
            if (message.Equals("acknowledged"))
            {
                Verify(json["packageId"].ToObject<int>(), json["scheduleId"].ToObject<int>());
            }
            #endregion

            #region unit-arrival
            else if (message.Equals("unit-arrival"))
            {
                ScheduleAt("unit-arrival", json["scheduleId"].ToObject<long>(), json["packageId"].ToObject<int>(), () =>
                {
                    var uid = json["uid"].ToObject<int>();
                    Registry.Ships[uid].GetComponent<PlayMakerFSM>().SendEvent("Arrive");
                });
            }
            #endregion

            #region move-unit
            else if (message.Equals("move-unit"))
            {
                var target = Registry.Islands[json["target"].ToObject<int>()];

                foreach (var shipUid in json["ships"].Select(shipId => shipId.ToObject<int>()))
                {
                    var uid = shipUid;
                    ScheduleAt("move-unit", json["scheduleId"].ToObject<long>(), json["packageId"].ToObject<int>(), () =>
                    {
                        // 1) add move component to ship
                        var move = Registry.Ships[uid].AddComponent<MoveToTargetMp>();
                        
                        // 2) change speed
                        move.Velocity = 150f;

                        // 3) set move destination
                        move.Target = target;

                        Debug.Log("move " + uid + " to " + target.GetComponent<IslandData>().Uid);
                    });
                }
            }
            #endregion

            #region spawn-units

            else if (message.Equals("spawn-unit"))
            {
                foreach (var spawn in json["spawns"])
                {
                    var ship = spawn;
                    Debug.Log("spawn units scheduled at: " + json["scheduleId"]);

                    ScheduleAt("spawn-unit", json["scheduleId"].ToObject<long>(), json["packageId"].ToObject<int>(), () =>
                    {
                        var shipUid = ship["uid"].ToObject<int>();
                        var island = Registry.Islands[ship["island_uid"].ToObject<int>()];
                        var islandData = island.GetComponent<IslandData>();

                        // 1) create ship by type
                        var go = GameObjectFactory.CreateShip(shipUid, islandData.ShipType, islandData.PlayerData.playerType);

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
                        if (IsHost())
                        {
                            go.AddComponent<Assault>();
                            shipData.AttackDamage = 2;
                            shipData.AttackSpeed = 1000;
                            go.AddComponent<Defence>();
                        }

                        // 7) re-enable spawning
                        island.GetComponentInChildren<SpawnUnitsMp>().ResetSpawnTimer();

//                        Debug.Log("spawn [uid=" + shipUid + "|type=" + shipData.shipType + "] at [uid=" + islandData.Uid + "|type=" + islandData.IslandType + "] for player [" + shipData.PlayerData.uid + "]");
                    });
                }
            }
            #endregion

            #region game-data
            else if(message.Equals("game-data"))
            {
                var gameData = json["game-data"];
                var playerDatas = gameData["players"];
                HostUid = gameData["host-uid"].ToString();
                Debug.Log("Player " + HostUid + " is host.");

                foreach (var playerDataRaw in playerDatas)
                {
                    var raw = playerDataRaw;
                    ExecuteOnMainThread.Enqueue(() =>
                        {
                            var player = GameObjectFactory.CreatePlayer(raw["uid"].ToString());
                            var playerData = player.GetComponent<PlayerData>();
                            playerData.playerType = PlayerData.GetPlayerType(raw["type"].ToString());

                            var islands = raw["islands"];
                            foreach (var data in islands)
                            {
                                var island = data;
                                var islandUid = island["uid"].ToObject<int>();
                                var islandType = island["type"].ToObject<int>();
                                var islandMaxSpawn = island["max-spawn"].ToObject<int>();

                                // 1) create island by type
                                var go = GameObjectFactory.CreateIsland(islandUid, islandType); 

                                // 2) set island transformation
                                var position = island["position"]; // island position
                                go.transform.position = new Vector3(position[0].ToObject<float>(), position[1].ToObject<float>(), position[2].ToObject<float>());
                                go.transform.localScale = new Vector3(Scale,Scale,Scale);

                                // 3) colorize
                                go.renderer.material.color = World.GetNextPlayerColor();

                                // 4) add meta data
                                var islandData = go.GetComponent<IslandData>();

                                // 4.1) set uid
                                islandData.Uid = islandUid;

                                // 4.2) island type
                                islandData.IslandType = islandType;

                                // 4.3) set ownership
                                islandData.PlayerData = playerData;
                                // Debug.Log(islandData.gameObject.name + " belongs to " + islandData.PlayerData.uid);

                                islandData.MaxSpawn = islandMaxSpawn;

                                // 4.4) life data
                                go.AddComponent<LifeData>();

                                // 4.5) set spawning ship types
                                islandData.ShipType = island["ship-type"].ToObject<int>();

                                // 4.6) host handles spawnings
                                if (IsHost())
                                {
                                    var spawnunits = go.transform.GetComponentInChildren<SpawnUnitsMp>();
                                    spawnunits.Island = islandData;
                                    islandData.SpawnRate = 3f;
                                    spawnunits.enabled = true;
                                }

                                // 4.7) set prototype ship
                                }
                        });
                }

                // when done, send client-game-ready command
                ExecuteOnMainThread.Enqueue(() => SocketHandler.Emit("client-game-ready", PackageFactory.CreateClientGameReadyMessage()));
            }
            #endregion

            #region authentification
            else if (message.Equals("Welcome!"))
            {
                ClientUid = json["uid"].ToString();

                SocketHandler.Emit("message", PackageFactory.CreateJoinQueueMessage(ClientUid));

                // create
                SocketHandler.Emit("join-game", PackageFactory.CreateGameTypeGameMessage("join-game", "game1vs1"));

                // join
                // SocketHandler.SharedConnection.Emit("create-game", PackageFactory.CreateGameTypeGameMessage("create-game", "game1vs1"));
            }
            #endregion

            #region handled somewhere else
            else if (message.Equals("latency"))
            {
                // handled in networkview
            }

            else if (message.Equals("ping"))
            {
                // handled in networkview
            }
            #endregion

            else
            {
                // todo more events 
                Debug.Log("Unhandled Message: " + json);
            }

        }
    }
}
