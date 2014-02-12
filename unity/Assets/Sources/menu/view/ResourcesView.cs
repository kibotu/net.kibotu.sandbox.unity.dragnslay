using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class ResourcesView : MonoBehaviour
    {
        public void Start()
        {
            var atlas = GameObject.Find("game_atlas").GetComponent<UIToolkit>();

            var sprite = UIButton.create(atlas, "resources.png", "resources.png", 0, 0);
            sprite.positionFromTop(0, 0);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);
        }
    }
}