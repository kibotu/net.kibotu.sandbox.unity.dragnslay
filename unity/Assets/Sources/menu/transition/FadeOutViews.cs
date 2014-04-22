using UnityEngine;

namespace Assets.Sources.menu.transition
{
    public class FadeOutViews : MonoBehaviour
    {
        public void Start()
        {
            foreach(var components in GetComponents<UiView>())
            {
                components.FadeOut();
            }
        }
    }
}
