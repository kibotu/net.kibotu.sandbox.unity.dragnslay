using Assets.net.kibotu.sandbox.unity.dragnslay.network;
using SimpleJson;
using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.scripts
{
    // @ see http://answers.unity3d.com/questions/34795/how-to-perform-a-mouse-click-on-game-object.html
    public class SendUnits : MonoBehaviour {

        public void Start()
        {
        }

        public void OnMouseDown()
        {
            var message = new JsonObject
                {
                    {"name", "move-units"},
                    {"source", gameObject.name},
                    {"dest", "2"},
                    {"amount", "1"}
                };
            ClientSocket.Instance.Emit("game-event", message);

            Debug.Log("game-event : " + message);
        }
    }
}
