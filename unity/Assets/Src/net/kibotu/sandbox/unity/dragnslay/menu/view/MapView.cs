using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class MapView : MonoBehaviour
    {
        public void Start()
        {
            var atlas = GameObject.Find("in_game_hud").GetComponent<UIToolkit>();

            var sprite = UIButton.create(atlas, "map.png", "map.png", 0, 0);
            sprite.positionFromTopRight(0, 0);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);
        }
    }
}