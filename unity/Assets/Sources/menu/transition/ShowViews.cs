using UnityEngine;

namespace Assets.Sources.menu.transition
{
    public class ShowViews : MonoBehaviour
    {
        public void Start()
        {
            foreach(var components in GetComponents<UiView>())
            {
                components.Show();
            }
        }
    }
}
