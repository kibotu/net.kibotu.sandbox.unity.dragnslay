using System;
using System.Collections;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    public class Game1vs1 : Game {
        
        protected void CreateWorld()
        {
            const float scale = 50;

            var island1 = GameObjectFactory.CreateIsland();
            island1.transform.position = new Vector3(50, 50, 0);
            island1.transform.localScale = new Vector3(scale, scale, scale);

            var island2 = GameObjectFactory.CreateIsland();
            island2.transform.Translate(50, 350  , 0);
            island2.transform.localScale = new Vector3(scale, scale, scale);

            var island3 = GameObjectFactory.CreateIsland();
            island3.transform.position = new Vector3(400, 50, 0);
            island3.transform.localScale = new Vector3(scale, scale, scale);

            var island4 = GameObjectFactory.CreateIsland();
            island4.transform.position = new Vector3(400, 350, 0);
            island4.transform.localScale = new Vector3(scale, scale, scale);

            //Planet [] p = new Planet[10] { n	ew Planet() };
            // add planets to stage

            // spawn ships
        }

        const float scale = 50;

        public override void OnStringEvent(string jsonMessage)
        {
            Debug.Log("Game1vs1 " + jsonMessage);

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
                        var go = GameObjectFactory.CreateIsland(Convert.ToInt32(island["type"])); // island type
                        go.GetComponent<SpawnUnits>().shipSpawnType = Convert.ToInt32(island["ship-type"]); // ship type
                        var position = (ArrayList)island["position"]; // position
                        go.transform.position = new Vector3(Convert.ToSingle(position[0]), Convert.ToSingle(position[1]), Convert.ToSingle(position[2])); 
                        go.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
            else if (message.Equals("Welcome!"))
            {
                Debug.Log(search["uid"]);
                SocketHandler.Instance.Emit("message", PackageFactory.CreateJoinQueueMessage((string)search["uid"]));

                // create
                SocketHandler.Instance.Emit("join-game", PackageFactory.CreateGameTypeGameMessage("join-game", "game1vs1"));

                // join
                // SocketHandler.Instance.Emit("create-game", PackageFactory.CreateGameTypeGameMessage("create-game", "game1vs1"));

                // request game data
                SocketHandler.Instance.Emit("request", PackageFactory.CreateRequestGameData());
            }
            else
            {
                // todo more events 
            }

        }

        public override void OnJSONEvent(JObject json)
        {
            var message = (string) json["message"];

            Debug.Log(message);
            
            if (message.Equals("move-units"))
            {
                Debug.Log("move units");
                Debug.Log("move units " + message);
                Debug.Log("move units " + json["target"]);
                Debug.Log("move units " + json["ships"]);
            }
            else if (message.Equals("spawn-units"))
            {
                Debug.Log("spawn units");

            }
            else if (message.Equals("game-data"))
            {
                Debug.Log("Receiving Game Data.");

                var gameData = json["game-data"];
                var players = gameData["players"];

                foreach (var player in players)
                {
                    Debug.Log("game data " + player["uid"]); // player uid
                    var islands = player["islands"];
                    foreach (var island in islands)
                    {
                        Debug.Log("game data island id " + island["uid"].ToObject<int>()); // island uid
                        Debug.Log("game data ship type " + island["ship-type"].ToObject<int>()); // ship type

                        JToken island1 = island;
                        ExecuteOnMainThread.Enqueue(() =>
                        {
                            var go = GameObjectFactory.CreateIsland(island1["type"].ToObject<int>()); // island type Newtonsoft.Json.Linq.JValue
                            go.GetComponent<SpawnUnits>().shipSpawnType = island1["ship-type"].ToObject<int>(); // ship type
                            var position = island1["position"]; // position
                            go.transform.position = new Vector3(position[0].ToObject<float>(), position[1].ToObject<float>(), position[2].ToObject<float>());
                            go.transform.localScale = new Vector3(scale, scale, scale);
                        });
                        
                    }
                }
            }
            else if (message.Equals("Welcome!"))
            {
                SocketHandler.Instance.Emit("message", PackageFactory.CreateJoinQueueMessage((string) json["uid"]));

                // create
                SocketHandler.Instance.Emit("join-game", PackageFactory.CreateGameTypeGameMessage("join-game", "game1vs1"));

                // join
                // SocketHandler.Instance.Emit("create-game", PackageFactory.CreateGameTypeGameMessage("create-game", "game1vs1"));
            }
            else if (message.Equals("server-game-ready"))
            {
                // request game data
                SocketHandler.Instance.Emit("request", PackageFactory.CreateRequestGameData());
            }
            else
            {
                // todo more events 
            }
        }
    }
}
