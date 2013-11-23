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
            else if (message.Equals("Welcome!"))
            {
                Debug.Log(search["uid"]);
                SocketHandler.Instance.SendMessage("send", PackageFactory.CreateJoinQueueMessage((string)search["uid"]));
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
