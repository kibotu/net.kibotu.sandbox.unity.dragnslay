using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    class SelfDestruct : MonoBehaviour
    {
        public void Start()
        {
            if (gameObject.particleSystem)
                Destroy(gameObject, GetComponent<ParticleSystem>().duration);
        }
    }
}
