using System.Collections;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.States;
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

        public void Start()
        {
            const float scale = 50;
            //start player 1
            var island1 = GameObjectFactory.CreateIsland();
            island1.transform.position = new Vector3(-150, 200, -20);
            island1.transform.localScale = new Vector3(scale, scale, scale);
            //start player 2
            var island2 = GameObjectFactory.CreateIsland();
            island2.transform.position = new Vector3(550, 200, -20);
            island2.transform.localScale = new Vector3(scale, scale, scale);

            var island3 = GameObjectFactory.CreateIsland();
            island3.transform.position = new Vector3(70, 50, 0);
            island3.transform.localScale = new Vector3(scale, scale, scale);

            var island4 = GameObjectFactory.CreateIsland();
            island4.transform.position = new Vector3(70, 350, 0);
            island4.transform.localScale = new Vector3(scale, scale, scale);

            var island5 = GameObjectFactory.CreateIsland();
            island5.transform.position = new Vector3(310, 50, 0);
            island5.transform.localScale = new Vector3(scale, scale, scale);

            var island6 = GameObjectFactory.CreateIsland();
            island6.transform.position = new Vector3(310, 350, 0);
            island6.transform.localScale = new Vector3(scale, scale, scale);


        }

        public override void OnStringEvent(string jsonMessage)
        {
            Debug.Log("Game1vs1 " + jsonMessage);

            var search = (IDictionary)MiniJSON.jsonDecode(jsonMessage);
            var message = search["message"];

            if (message.Equals("move-units"))
            {
                Debug.Log("move units");
            } 
            else if (message.Equals("spawn-units"))
            {
                Debug.Log("spawn units");
            }
            else if (message.Equals("game-data"))
            {
                Debug.Log("game data");

                Debug.Log("game data: " + search);
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
