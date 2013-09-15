using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.scripts
{
    public class Orbitting : MonoBehaviour
    {
        public float rotateVelocity;
        private Transform pivot;

        void Awake()
        {
            rotateVelocity = 1f;
            pivot = transform;
        }

        void Start () {
        }
	
        void Update () {
            transform.Rotate(new Vector3(0,1,0) * Time.deltaTime * 100, Space.World);
            //transform.RotateAround(pivot.position, Vector3.right, rotateVelocity * Time.deltaTime);
        }
    }
}
