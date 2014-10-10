using UnityEngine;

namespace Assets.Sources.components
{
    public class AnimateTexture : MonoBehaviour {

        public float ScrollSpeed = 0.5f;

        void Update() {
            var offset = Time.time * ScrollSpeed;
            renderer.material.mainTextureOffset = new Vector2(offset%1, 0);
        } 
    }
}
