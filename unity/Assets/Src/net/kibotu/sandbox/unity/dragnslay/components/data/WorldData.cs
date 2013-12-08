using System.Collections.Generic;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data
{
    public class WorldData : MonoBehaviour {
	
        public List<PlayerData> PlayerData;
        public string user;
        
        public void Awake()
        {
            PlayerData = new List<PlayerData>();
        }

        public IEnumerable<Color> GetPlayerColor()
        {
            yield return Color.green;
            yield return Color.red;
            yield return Color.yellow;
        }

        private static int playerColorNumber = 0;

        public Color GetNextPlayerColor()
        {
            switch (playerColorNumber)
            {
                case 0:
                    playerColorNumber++;
                    return Color.green;
                case 1:
                    playerColorNumber++;
                    return Color.red;
                case 2:
                    playerColorNumber++;
                    return Color.yellow;
                default:
                    return Color.clear;
            }
        }
    }
}