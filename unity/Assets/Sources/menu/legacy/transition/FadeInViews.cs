using UnityEngine;

namespace Assets.Sources.menu.transition
{
    public class FadeInViews : MonoBehaviour
    {
        public void Start()
        {
            foreach(var components in GetComponents<UiView>())
            {
                components.FadeIn();
            }
        }
    }
}
