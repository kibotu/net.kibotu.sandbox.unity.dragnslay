using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.model
{
    public class PhysicalProperty {
	
        public Vector3 position;
        public Vector3 scalling;
        public Quaternion rotation;
	
        public float acceleration;
        public float mass;
        public float rotationSpeed;
        public float rotationDistance;
	
        public float speed(int t, int v0) {
            return acceleration * t + v0;
        }
    }
}

