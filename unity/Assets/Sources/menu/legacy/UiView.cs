using System.Collections;
using UnityEngine;

namespace Assets.Sources.menu
{
    public abstract class UiView : MonoBehaviour
    {
        public abstract void FadeIn();
        public abstract void FadeOut();
        public abstract void Show();
        public abstract void Hide();
    }
}
