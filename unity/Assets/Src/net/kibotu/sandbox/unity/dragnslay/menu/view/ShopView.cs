using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class ShopView : MonoBehaviour
    {
        public void Start()
        {
            var atlas = GameObject.Find("shop_atlas").GetComponent<UIToolkit>();

            var sprite = UIButton.create(atlas, "center.png", "center.png", 0, 0);
            sprite.positionCenter();
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);

            sprite = UIButton.create(atlas, "buy.png", "buy.png", 0, 0);
            sprite.positionFromBottom(0.2f);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);

          //  sprite = UIButton.create(atlas, "left.png", "left.png", 0, 0);
          //  sprite.positionFromLeft(0.27f);
          //  sprite.scale = new Vector3(0.52083f, 0.520833f, 0);

          //  sprite = UIButton.create(atlas, "right.png", "right.png", 0, 0);
         //   sprite.positionFromRight(0.27f);
          //  sprite.scale = new Vector3(0.52083f, 0.520833f, 0);

            sprite = UIButton.create(atlas, "Skins.png", "Skins.png", 0, 0);
            sprite.positionFromCenter(-0.2f,-0.21f);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);

            sprite = UIButton.create(atlas, "boosts.png", "boosts.png", 0, 0);
            sprite.positionFromCenter(-0.2f, -0.07f);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);

            sprite = UIButton.create(atlas, "upgrades.png", "upgrades.png", 0, 0);
            sprite.positionFromCenter(-0.2f, 0.07f);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);

            sprite = UIButton.create(atlas, "buffs.png", "buffs.png", 0, 0);
            sprite.positionFromCenter(-0.2f, 0.21f);
            sprite.scale = new Vector3(0.52083f, 0.520833f, 0);
        }
    }
}