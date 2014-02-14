using UnityEngine;

namespace Assets.Sources.menu.view
{
    public class ShopView : MonoBehaviour
    {   
        float scale = 1f;
        public void Start()
        {
            var atlas = GameObject.Find("shop_atlas").GetComponent<UIToolkit>();

            var sprite = UIButton.create(atlas, "center.png", "center.png", 0, 0);
            sprite.positionCenter();
            sprite.scale = new Vector3(scale, scale, 1);

            sprite = UIButton.create(atlas, "buy.png", "buy.png", 0, 0);
            sprite.positionFromCenter(0.39f,0f);
            sprite.scale = new Vector3(scale, scale, 1);

            sprite = UIButton.create(atlas, "Skins.png", "Skins.png", 0, 0);
            sprite.positionFromCenter(-0.4f,-0.375f);
            sprite.scale = new Vector3(scale, scale, 1);

            sprite = UIButton.create(atlas, "boosts.png", "boosts.png", 0, 0);
            sprite.positionFromCenter(-0.4f, -0.125f);
            sprite.scale = new Vector3(scale, scale, 1);

            sprite = UIButton.create(atlas, "upgrades.png", "upgrades.png", 0, 0);
            sprite.positionFromCenter(-0.4f, 0.125f);
            sprite.scale = new Vector3(scale, scale, 1);

            sprite = UIButton.create(atlas, "buffs.png", "buffs.png", 0, 0);
            sprite.positionFromCenter(-0.4f, 0.375f);
            sprite.scale = new Vector3(scale, scale, 1);
        }
    }
}