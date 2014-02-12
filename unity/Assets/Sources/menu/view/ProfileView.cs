using System;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.menu.view
{
    public class ProfileView : MonoBehaviour
    {
        private UIButton sprite;

        public void Start()
        {
            var atlas = GameObject.Find("profile_atlas").GetComponent<UIToolkit>();

            sprite = UIButton.create(atlas, "profile.png", "profile.png", 0, 0);
            sprite.positionCenter();
            sprite.onTouchUpInside += OnUpgradesPressed;
            //sprite.scale = new Vector3(0.52083f, 0.520833f, 0);
        }

        public void OnUpgradesPressed(UIButton button)
        {
            sprite.destroy();
            GameObject.Find("Menu").GetComponent<Menu>().ShowUpgrades();
            Destroy(this);
        }
    }
}