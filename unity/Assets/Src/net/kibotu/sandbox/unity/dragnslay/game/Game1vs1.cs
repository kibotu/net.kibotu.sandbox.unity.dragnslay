using System;
using System.Collections;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.States;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using SimpleJson;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.game
{
    public class Game1vs1 : Game {
        
        protected override void CreateWorld()
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

        public void Start()
        {
            // do stuff on first start
        }

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
                // SocketHandler.Instance.SendMessage("create-game", PackageFactory.CreateGameTypeGameMessage("create-game", "game1vs1"));

                // request game data
                SocketHandler.Instance.Emit("request", PackageFactory.CreateRequestGameData());
            }
            else
            {
                // todo more events 
            }

        }

        public override void OnJSONEvent(string message)
        {
            Debug.Log("Game1vs1 json " + message);
        }
    }
}
