using Assets.Sources.components.data;
using Assets.Sources.model;
using UnityEngine;
using System.Collections.Generic;
using Assets.Sources.utility;

namespace Assets.Sources.components.behaviours.ai
{
    public class Thinking : MonoBehaviour {

		private PlayMakerFSM fsm;

		public void Start() 
		{
			fsm = GetComponent<PlayMakerFSM> ();
		}

        public void Think()
        {
            var playerData = GetComponent<PlayerData>();

			// lists of islands by player
			// start of by only looking for non owned islands, later we could prioritize weaker targets
			List<IslandData> ownedIslands = new List<IslandData> ();
			List<IslandData> enemyIslands = new List<IslandData> ();
			foreach (var island in Registry.Islands.Values)
			{
				var islandData = island.GetComponent<IslandData>();
				
				if (islandData.PlayerData.uid != playerData.uid)
					enemyIslands.Add(islandData);
				else 
					ownedIslands.Add(islandData);
			}

			Debug.Log (enemyIslands.Count + " enemies vs owned " + ownedIslands.Count);

            // no enemies or owning no islands do nothing
			if (enemyIslands.IsEmpty () || ownedIslands.IsEmpty ()) 
			{
				fsm.SendEvent("Idle");
				return;
			}

            // use boost

            // defend

            // attack
			MoveUnitsTo(ownedIslands.GetRandom().gameObject, enemyIslands.GetRandom ().gameObject);
			fsm.SendEvent("Attack");

			// Debug.Log("enemy island: " + islandData.PlayerData.uid + " != " + playerData.uid);
        }

        public void MoveUnitsTo(GameObject source, GameObject target)
        {
            // 1) already there
            if (target == null || target == source) return;

            // 3) send own units to destination
            for (var i = 0; i < source.transform.childCount; ++i)
            {
                var papership = source.transform.GetChild(i);
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
