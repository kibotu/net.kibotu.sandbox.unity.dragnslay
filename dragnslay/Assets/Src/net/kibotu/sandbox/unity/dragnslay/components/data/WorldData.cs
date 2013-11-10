using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data
{
    public class WorldData : MonoBehaviour {
	
        public List<PlayerData> PlayerData;

        public void Awake()
        {
            PlayerData = new List<PlayerData>();
        }
    }
}

