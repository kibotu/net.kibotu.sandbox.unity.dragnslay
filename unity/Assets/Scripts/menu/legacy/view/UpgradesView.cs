using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class UpgradesView : MonoBehaviour
    {
        public void Start()
        {
            var atlas = GameObject.Find("profile_atlas").GetComponent<UIToolkit>();

            var sprite = UIButton.create(atlas, "boosts.png", "boosts.png", 0, 0);
            sprite.positionCenter();
            //sprite.scale = new Vector3(0.52083f, 0.520833f, 0);
        }
    }
}