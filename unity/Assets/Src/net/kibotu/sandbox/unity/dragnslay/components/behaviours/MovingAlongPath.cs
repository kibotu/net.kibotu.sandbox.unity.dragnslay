using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    class MovingAlongPath : MonoBehaviour
    {
        public float t;
        public Transform[] path;

        public void Update()
        {
            transform.position = Spline.MoveOnPath(path, transform.position, ref t, 0.5f);
        }
    }
}
