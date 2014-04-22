using UnityEngine;

namespace Assets.Sources.menu.transition
{
    public class HideViews : MonoBehaviour
    {
        public void Start()
        {
            foreach(var components in GetComponents<UiView>())
            {
                components.Hide();
            }
        }
    }
}
