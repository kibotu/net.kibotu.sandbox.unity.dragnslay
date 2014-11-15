using UnityEngine;

namespace Assets.Sources.components.data
{
    public class PhysicData {
	
        public Vector3 position;
        public Vector3 scalling;
        public Quaternion rotation;
	
        public float acceleration;
        public float mass;
        public float rotationSpeed;
        public float rotationDistance;

        public PhysicData(Vector3 position, Vector3 scalling, Quaternion rotation, float acceleration, float mass, float rotationSpeed, float rotationDistance)
        {
            this.position = position;
            this.scalling = scalling;
            this.rotation = rotation;
            this.acceleration = acceleration;
            this.mass = mass;
            this.rotationSpeed = rotationSpeed;
            this.rotationDistance = rotationDistance;
        }

        public float speed(int t, int v0) {
            return acceleration * t + v0;
        }
    }
}

