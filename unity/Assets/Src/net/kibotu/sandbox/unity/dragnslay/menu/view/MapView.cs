using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MapView : MonoBehaviour
    {
        public void Start()
        {
            var in_game_hud = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            var map = UIButton.create(in_game_hud, "map.png", "map.png", 0,0);
            map.positionFromTopRight(0, 0);
            map.scale = new Vector3(0.6f, 0.6f, 0.6f);
        }
    }
}