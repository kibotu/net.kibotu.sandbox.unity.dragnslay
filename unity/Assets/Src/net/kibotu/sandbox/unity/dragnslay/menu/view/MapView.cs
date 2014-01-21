using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MapView : MonoBehaviour
    {
        void Start()
        {
            var in_game_hud = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            var map = UIButton.create(in_game_hud, "map.png", "map.png", Screen.width-239,Screen.height-150);
            map.centerize();
            map.scale = new Vector3(0.6f, 0.6f, 0.6f);
            map.position = new Vector2(571, 0);
        }
    }
}