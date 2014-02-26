using Assets.Sources.components.data;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.components.behaviours.ai
{
    public class Thinking : MonoBehaviour {

        public void Think()
        {
            var playerData = GetComponent<PlayerData>();

            // do nothing

            // use boost

            // defend

            // attack

            foreach (var island in Registry.Islands.Values)
            {
                var islandData = island.GetComponent<IslandData>();
                if (islandData.PlayerData.uid != playerData.uid)
                {
                    Debug.Log("enemy island: " + islandData.PlayerData.uid + " != " + playerData.uid);



                    GetComponent<PlayMakerFSM>().SendEvent("Attack");
                }
            }
        }

        public void MoveUnitsTo(GameObject source, GameObject target)
        {
            // 1) already there
            if (target == null || target == source) return;

            // 3) send own units to destination
            for (var i = 0; i < transform.childCount; ++i)
            {
                var papership = transform.GetChild(i);
                if (papership.name.StartsWith("Papership"))
                {
                    if (papership.GetComponent<PlayMakerFSM>().ActiveStateName != "Moving")
                    {
                        var move = papership.gameObject.AddComponent<MoveToTarget>();
                        move.Target = target;
                    }
                }
            }
        }
    }
}
