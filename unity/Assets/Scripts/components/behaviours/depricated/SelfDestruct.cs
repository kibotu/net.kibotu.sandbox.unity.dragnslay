using System;
using UnityEngine;

namespace Assets.Sources.components.behaviours.depricated
{
    [Obsolete("Not used anymore", false)]
    class SelfDestruct : MonoBehaviour
    {
        public void Start()
        {
            if (gameObject.particleSystem)
                Destroy(gameObject, GetComponent<ParticleSystem>().duration);
        }
    }
}
