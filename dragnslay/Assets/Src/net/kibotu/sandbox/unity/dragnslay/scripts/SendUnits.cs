using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.scripts
{
    // @ see http://answers.unity3d.com/questions/34795/how-to-perform-a-mouse-click-on-game-object.html
    public class SendUnits : MonoBehaviour {

        public void Start()
        {
        }

        public void OnMouseDown()
        {
            // SocketHandler.Instance.Emit("send", SocketHandler.Instance.createSendUnitsMessage());
            
        }
    }
}
