using System.Collections.Generic;
using Assets.Sources.game;
using Assets.Sources.menu;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.components.data
{
    public class WorldData : MonoBehaviour
    {
        public List<GameObject> Player;
        
        public void Awake()
        {
            Registry.Init();

            Player = new List<GameObject>();


            if (Game.IsSinglePlayer())
            {
                var player = GameObjectFactory.CreatePlayer("player");
                player.GetComponent<PlayerData>().playerType = PlayerData.PlayerType.Player;
                player.transform.parent = transform;
                player.GetComponent<PlayerData>().color = new Color(0f / 255, 0, 155f / 255) + new Color(0.4f, 0.4f, 0.4f);
                Player.Add(player);

                player = GameObjectFactory.CreatePlayer("neutral");
                player.GetComponent<PlayerData>().playerType = PlayerData.PlayerType.Neutral;
                player.transform.parent = transform;
                player.GetComponent<PlayerData>().color = new Color(155f/255, 140f/255, 60f/255) + new Color(0.4f,0.4f,0.4f);
                Player.Add(player);

                player = GameObjectFactory.CreateAi("offensive");
                player.GetComponent<PlayerData>().playerType = PlayerData.PlayerType.Offensive;
                player.transform.parent = transform;
                player.GetComponent<PlayerData>().color = new Color(155f/255, 0f/255, 0f/255) + new Color(0.6f,0.6f,0.6f);
                Player.Add(player);

                // GameObject.Find("Menu").GetComponent<Menu>().ShowGameHud();
            }
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