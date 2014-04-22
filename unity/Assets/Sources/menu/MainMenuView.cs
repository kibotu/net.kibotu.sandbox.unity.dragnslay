using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.menu
{
    public class MainMenuView : UiView
    {
        public UIButton Banner;
        public UIButton TrainingButton;
        public UIButton MultiplayerButton;
        public UIButton SettingsButton;
        public UIButton ProfileButton;
        public UIButton ShopButton;

        public static Color Invisible = new Color(1f, 1f, 1f, 0f);
        public static Color Visible = new Color(1f, 1f, 1f, 1f);

        public void Start()
        {
            var textureAtlas = GameObject.Find("ui_menu").GetComponent<UIToolkit>();

            Banner = UIButton.create(textureAtlas, "banner.png", "banner.png", 0, 0);
            Banner.positionFromTop(0.04f, 0);
            Banner.scale = new Vector3(0.7f, 0.7f, 1);

            var scale = new Vector3(0.3f, 0.3f, 1);
            var topMargin = 0.7f;

            //var textAtlas = new UIText(GameObject.Find("ui_text").GetComponent<UIToolkit>(),"prototype", "prototype.png");
            TrainingButton = UIButton.create(textureAtlas, "button.png", "button.png", 0, 0);
            TrainingButton.positionFromTop(topMargin, -0.3f);
            TrainingButton.scale = scale;
            TrainingButton.onTouchUp += button => Application.LoadLevel((int) Registry.Levels.Training);

            MultiplayerButton = UIButton.create(textureAtlas, "button.png", "button.png", 0, 0);
            MultiplayerButton.positionFromTop(topMargin, -0.1f);
            MultiplayerButton.scale = scale;

            ProfileButton = UIButton.create(textureAtlas, "button.png", "button.png", 0, 0);
            ProfileButton.positionFromTop(topMargin, 0.1f);
            ProfileButton.scale = scale;

            ShopButton = UIButton.create(textureAtlas, "button.png", "button.png", 0, 0);
            ShopButton.positionFromTop(topMargin, 0.3f);
            ShopButton.scale = scale;
        }

        public float timeScale = 1f;

        public void Update()
        {
            Time.timeScale = timeScale;
        }

        public override void FadeIn()
        {
            var toScale = new Vector3(0.35f, 0.35f, 1);
            var fadeInTime = 7f;
            var scaleToTime = 35f;

            Banner.colorFromTo(fadeInTime, Invisible, Visible, Easing.Quartic.easeOut);
            Banner.scaleTo(scaleToTime, new Vector3(0.9f, 0.9f, 1), Easing.Linear.easeOut);

            TrainingButton.colorFromTo(fadeInTime, Invisible, Visible, Easing.Quartic.easeOut);
            TrainingButton.scaleTo(scaleToTime, toScale, Easing.Linear.easeOut);

            MultiplayerButton.colorFromTo(fadeInTime, Invisible, Visible, Easing.Quartic.easeOut);
            MultiplayerButton.scaleTo(scaleToTime, toScale, Easing.Linear.easeOut);

            ProfileButton.colorFromTo(fadeInTime, Invisible, Visible, Easing.Quartic.easeOut);
            ProfileButton.scaleTo(scaleToTime, toScale, Easing.Linear.easeOut);

            ShopButton.colorFromTo(fadeInTime, Invisible, Visible, Easing.Quartic.easeOut);
            ShopButton.scaleTo(scaleToTime, toScale, Easing.Linear.easeOut);
        }

        public override void FadeOut()
        {
            throw new System.NotImplementedException();
        }

        public override void Show()
        {
            Banner.color = Visible;
            TrainingButton.color = Visible;
            MultiplayerButton.color = Visible;
            //SettingsButton.color = Visible;
            ProfileButton.color = Visible;
            ShopButton.color = Visible;
        }

        public override void Hide()
        {
            Banner.color = Invisible;
            TrainingButton.color = Invisible;
            MultiplayerButton.color = Invisible;
            //SettingsButton.color = Invisible;
            ProfileButton.color = Invisible;
            ShopButton.color = Invisible;
        }
    }
}

