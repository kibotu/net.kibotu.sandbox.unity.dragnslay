using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    class MovingAlongPath : MonoBehaviour
    {
        public float t;
        public Vector3[] VPath;
        public GameObject path;
        private Transform []TPath;
        public float speed;

        public void Start()
        {
        }

        public void Update()
        {

            if (TPath == null)
                TPath = path.GetComponentsInChildren<Transform>();

            transform.position = path != null ? Spline.MoveOnPath(TPath, transform.position, ref t, speed) : Spline.MoveOnPath(VPath, transform.position, ref t, speed);
        }
    }
}
