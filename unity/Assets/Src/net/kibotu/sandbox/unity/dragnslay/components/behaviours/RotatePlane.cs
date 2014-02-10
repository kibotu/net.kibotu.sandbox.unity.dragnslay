using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
{
    class RotatePlane : MonoBehaviour
    {
        /*public float speed;
        public float radius;
        public float distance;
        public Vector3 rotation;
        public Vector3 localPosition;
        
        public void Start()
        {
            speed = 20f;
            radius = 10;
            distance = 10;
            rotation = new Vector3();

            transform.localPosition = localPosition = new Vector3(0,2,0);

            //transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public void Update()
        {
            if (!Game.IsRunning()) return;

            //transform.localPosition = localPosition;
            transform.RotateAround(transform.parent.position, new Vector3(0, 0, 1), Time.deltaTime * speed);
            var pos = transform.localPosition;
            pos.z += 100;
            transform.position = pos;
        }

        public Vector3 GetRotation()
        {
            var parent = gameObject.transform.parent.position;
            rotation.x = radius * Mathf.Sin(Time.deltaTime);
            rotation.y = parent.y + distance;
            rotation.z = radius * Mathf.Cos(Time.deltaTime);
            return rotation;
        }

        public static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle) {
            return angle * ( point - pivot) + pivot;
        }*/

        public Transform center;
        public Vector3 axis = new Vector3(0,1,0);
        public Vector3 desiredPosition;
        public float radius = 100f;
        public float radiusSpeed = 0.5f;
        public float rotationSpeed = 80.0f;
        public float cameraSpeed = 10f;

        public void Start()
        {
            center = transform.parent.GetChild(transform.parent.childCount - 1).transform;
           // transform.rotation = Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0,-75,0);
            transform.localPosition = new Vector3(60, 0, 40);
            transform.position = (transform.position - center.position).normalized * radius + center.position;
            
        }

        public void Update()
        {
            center = transform.parent.GetChild(transform.parent.childCount-1).transform;
            transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
            desiredPosition = (transform.position - center.position).normalized * radius + center.position;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
        }

        public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle) {
            var dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angle) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; 
        }
    }
}
