using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    public class Rotating : MonoBehaviour
    {
        public float RotateVelocity;
        private Transform _pivot;

        public void Awake()
        {
            RotateVelocity = 1f;
            _pivot = transform;
        }

        public void Start () {
        }
	
        public void Update ()
        {
            if (!Game.IsRunning()) return;

            transform.Rotate(new Vector3(0,1,1) * Time.deltaTime * 100, Space.World);
            //transform.RotateAround(pivot.position, Vector3.right, rotateVelocity * Time.deltaTime);
        }
    }
}
