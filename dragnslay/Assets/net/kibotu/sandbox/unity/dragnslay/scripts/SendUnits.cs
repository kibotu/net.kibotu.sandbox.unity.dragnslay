using Assets.net.kibotu.sandbox.unity.dragnslay.network;
using SimpleJson;
using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.scripts
{
    public class SendUnits : MonoBehaviour {

        public void OnMouseDown()
        {
            Debug.Log("clicked");
            /*
            JsonObject message = new JsonObject();
            message.Add("name", "move-units");
            message.Add("source", this.gameObject.name);
            message.Add("dest", "2");
            message.Add("amount", "1");

            ClientSocket.Instance.Emit("game-event", message);

            Debug.Log("game-event : " + message.ToString());*/
        }
    }
}
