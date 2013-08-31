using SimpleJson;
using UnityEngine;

public class SendUnits : MonoBehaviour {

    public void OnMouseDown()
    {
		Debug.Log("OnMouseUp");

        JsonObject message = new JsonObject();
        message.Add("name", "move-units");
        message.Add("source", this.gameObject.name);
        message.Add("dest", "2");
        message.Add("amount", "1");

        ClientSocket.Instance.Emit("game-event", message);
	}
}
