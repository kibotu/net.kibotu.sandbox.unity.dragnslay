using Assets.Sources.components.behaviours.combat;
using Assets.Sources.components.behaviours.legacy;
using Assets.Sources.components.behaviours.multiplayer;
using Assets.Sources.components.data;
using Assets.Sources.model;
using Assets.Sources.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Sources.game
{
    public class Game1vs1 : GameMp {

        const float Scale = 20;

        protected override void DoGameTurn()
        {
            Debug.Log("Game-Loop Turn: " + Turn);
        }

        public override void OnJSONEvent(JObject json)
        {
            //Debug.Log("message : " + json);

            var message = json["message"].ToString();

            #region move-unit
            if (message.Equals("move-unit"))
            {
                var target = Registry.Instance.Islands[json["target"].ToObject<int>()];

                foreach (var shipId in json["ships"])
                {
                    var ship_uid = shipId.ToObject<int>();
                    ExecuteOnMainThread.Enqueue(() =>
                    {
                        // 1) add move component to ship
                        var move = Registry.Instance.Ships[ship_uid].AddComponent<Move>();
                        move.speed = 25;

                        // 2) set move destination
                        move.destination = target.transform.FindChild("Sphere");

                        Debug.Log("move " + ship_uid + " to " + target.GetComponent<IslandData>().uid);
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
                    ExecuteOnMainThread.Enqueue(() =>
                    {
                        var shipUid = ship["uid"].ToObject<int>();
                        var island = Registry.Instance.Islands[ship["island_uid"].ToObject<int>()];
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
                        if (IsHost())
                        {
                            var attack = go.AddComponent<Assault>();
                            attack.AttackDamage = 2;
                            attack.AttackSpeed = 1000;
                            go.AddComponent<Defence>();
                        }

                        Debug.Log("spawn [uid=" + shipUid + "|type=" + shipData.shipType + "] at [uid=" + islandData.uid + "|type=" + islandData.islandType  + "] for player [" + shipData.playerUid + "]");
                    });
                }
            }
            #endregion

            #region game-data
            else if(message.Equals("game-data"))
            {
                var gameData = json["game-data"];
                var players = gameData["players"];
                HostUid = gameData["host-uid"].ToString();
                Debug.Log("Player " + HostUid + " is host.");

                foreach (var player in players)
                {
                    var playerData = new PlayerData {uid = player["uid"].ToString(), color = World.GetNextPlayerColor()};
                    World.PlayerData.Add(playerData);

                    var islands = player["islands"];
                    foreach (var data in islands)
                    {
                        var island = data;
                        ExecuteOnMainThread.Enqueue(() =>
                        {
                            var islandUid = island["uid"].ToObject<int>();
                            var islandType = island["type"].ToObject<int>();
                            var islandMaxSpawn = island["max-spawn"].ToObject<int>();

                            // 1) create island by type
                            var go = GameObjectFactory.CreateIsland(islandUid, islandType); 

                            // 2) set island transformation
                            var position = island["position"]; // island position
                            go.transform.position = new Vector3(position[0].ToObject<float>(), position[1].ToObject<float>(), position[2].ToObject<float>());
                            go.transform.localScale = new Vector3(Scale, Scale, Scale);

                            // 3) colorize
                            go.renderer.material.color = playerData.color;

                            // 4) add meta data
                            var islandData = go.AddComponent<IslandData>();

                            // 4.1) set uid
                            islandData.uid = islandUid;

                            // 4.2) island type
                            islandData.islandType = islandType;

                            // 4.3) set ownership
                            islandData.playerUid = playerData.uid;

                            islandData.maxSpawn = islandMaxSpawn;

                            // 4.4) life data
                            go.AddComponent<LifeData>();

                            // 4.5) set spawning ship types
                            islandData.shipType = island["ship-type"].ToObject<int>();

                            // 4.6) host handles spawnings
                            if(IsHost())
                                go.AddComponent<SpawnUnitsMp>();

                            // 4.7) set prototype ship

                        });
                    }
                }

                // when done, send client-game-ready command
                ExecuteOnMainThread.Enqueue(() => SocketHandler.Emit("client-game-ready", PackageFactory.CreateClientGameReadyMessage()));
            }
            #endregion

            else if (message.Equals("start-game"))
            {
                StartGame();
            }
            else if (message.Equals("pause-game"))
            {
                PauseGame();
            }
            else if (message.Equals("resume-game"))
            {
                ResumeGame();
            }
            else if (message.Equals("stop-game"))
            {
                StopGame();
            }

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

            else if (message.Equals("server-game-ready"))
            {
                // request game data
                SocketHandler.Emit("request", PackageFactory.CreateRequestGameData());
            }
            else if (message.Equals("waiting-for-player"))
            {
                foreach (var uid in json["player"])
                {
                    Debug.Log("Waiting for player: " + uid);
                }
            }
            else
            {
                // todo more events 
                Debug.Log("Unhandled Message: " + json);
            }
        }
    }
}
