using System;
using System.Collections;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    public class Game1vs1 : Game {

        const float scale = 50;

        public override void OnStringEvent(string jsonMessage)
        {
            var search = (IDictionary)MiniJSON.jsonDecode(jsonMessage);
            var message = search["message"];

            if (message.Equals("move-units"))
            {
                Debug.Log("move units");
                Debug.Log("move units " + message);
                Debug.Log("move units " + search["target"]);
                Debug.Log("move units " + search["ships"]);
            } 
            else if (message.Equals("spawn-units"))
            {
                Debug.Log("spawn units");
                
            }
            else if (message.Equals("game-data"))
            {
                Debug.Log("Receiving Game Data.");

                var gameData = (Hashtable) search["game-data"];
                var players = (ArrayList) gameData["players"];
                foreach (Hashtable player in players)
                {
                    Debug.Log("game data " + player["uid"]); // uid
                    var islands = (ArrayList) player["islands"];
                    foreach (Hashtable island in islands)
                    {
                        Debug.Log("game data island id " + Convert.ToInt32(island["uid"])); // uid
                        //var go = GameObjectFactory.CreateIsland(Convert.ToInt32(island["type"])); // island type
                        //go.GetComponent<SpawnUnits>().shipSpawnType = Convert.ToInt32(island["ship-type"]); // ship type
                        var position = (ArrayList)island["position"]; // position
                       // go.transform.position = new Vector3(Convert.ToSingle(position[0]), Convert.ToSingle(position[1]), Convert.ToSingle(position[2])); 
                        //go.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
            else if (message.Equals("Welcome!"))
            {
                ClientUid = (string) search["uid"];
                SocketHandler.SharedConnection.Emit("message", PackageFactory.CreateJoinQueueMessage(ClientUid));

                // create
                SocketHandler.SharedConnection.Emit("join-game", PackageFactory.CreateGameTypeGameMessage("join-game", "game1vs1"));

                // join
                // SocketHandler.SharedConnection.Emit("create-game", PackageFactory.CreateGameTypeGameMessage("create-game", "game1vs1"));

                // request game data
                SocketHandler.SharedConnection.Emit("request", PackageFactory.CreateRequestGameData());
            }
            else
            {
                // todo more events 
            }

        }

        public override void OnJSONEvent(JObject json)
        {
            var message = (string) json["message"];
            
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

                        // 4) colorize
                        go.renderer.material.color = island.renderer.material.color;

                        Debug.Log("spawn [uid=" + shipUid + "|type=" + shipData.shipType + "] at [uid=" + islandData.uid + "|type=" + islandData.islandType  + "] for player [" + shipData.playerUid + "]");
                    });
                }
            }
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
                    foreach (var island in islands)
                    {
                        var data = island;
                        ExecuteOnMainThread.Enqueue(() =>
                        {
                            var islandUid = data["uid"].ToObject<int>();
                            var islandType = data["type"].ToObject<int>();

                            // 1) create island by type
                            var go = GameObjectFactory.CreateIsland(islandUid, islandType); 

                            // 2) set island transformation
                            var position = data["position"]; // island position
                            go.transform.position = new Vector3(position[0].ToObject<float>(), position[1].ToObject<float>(), position[2].ToObject<float>());
                            go.transform.localScale = new Vector3(scale, scale, scale);

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

                            // 4.4) life data
                            go.AddComponent<LifeData>();

                            // 4.5) set spawning ship types
                            islandData.shipType = data["ship-type"].ToObject<int>();

                            // 4.6) host handles spawnings
                            if(HostUid == ClientUid)
                                go.AddComponent<SpawnUnits>();
                        });
                    }
                }
            }
            else if (message.Equals("Welcome!"))
            {
                ClientUid = json["uid"].ToString();

                SocketHandler.SharedConnection.Emit("message", PackageFactory.CreateJoinQueueMessage(ClientUid));

                // create
                SocketHandler.SharedConnection.Emit("join-game", PackageFactory.CreateGameTypeGameMessage("join-game", "game1vs1"));

                // join
                // SocketHandler.SharedConnection.Emit("create-game", PackageFactory.CreateGameTypeGameMessage("create-game", "game1vs1"));
            }
            else if (message.Equals("server-game-ready"))
            {
                // request game data
                SocketHandler.SharedConnection.Emit("request", PackageFactory.CreateRequestGameData());
            }
            else
            {
                // todo more events 
            }
        }
    }
}
