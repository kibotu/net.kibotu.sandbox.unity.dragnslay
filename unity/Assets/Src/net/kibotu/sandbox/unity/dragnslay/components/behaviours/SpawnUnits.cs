using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.network;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    public class SpawnUnits : MonoBehaviour
    {
        public void Update ()
        {
            if (GetComponentsInChildren<Transform>().Length >= 3) return;
                SocketHandler.SharedConnection.Emit("spawn-unit", PackageFactory.CreateSpawnMessage(new[] { new SpawnData { island_uid = gameObject.GetComponent<IslandData>().uid, uid = -1 } }));
        }
    }
}
