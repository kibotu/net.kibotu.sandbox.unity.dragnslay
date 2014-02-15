using System;
using UnityEngine;

namespace Assets.Sources.components.behaviours.legacy
{
    [Obsolete("Not used anymore", false)]
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

           // transform.position = path != null ? Spline.MoveOnPath(TPath, transform.position, ref t, speed) : Spline.MoveOnPath(VPath, transform.position, ref t, speed);
        }
    }
}
