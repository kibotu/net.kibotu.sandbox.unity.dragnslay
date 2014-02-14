using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.components.data
{
    public class WorldData : MonoBehaviour {
	
        public List<PlayerData> PlayerData;
        
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

        private static int _playerColorNumber = 0;

        public Color GetNextPlayerColor()
        {
            switch (_playerColorNumber)
            {
                case 0:
                    _playerColorNumber++;
                    return Color.green;
                case 1:
                    _playerColorNumber++;
                    return Color.red;
                case 2:
                    _playerColorNumber++;
                    return Color.yellow;
                default:
                    return Color.clear;
            }
        }
    }
}