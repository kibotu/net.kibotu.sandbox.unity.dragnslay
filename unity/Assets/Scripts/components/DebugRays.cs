using Assets.Sources.utility;
using UnityEngine;

namespace Assets.Sources.components
{
    public class DebugRays : MonoBehaviour
    {
        public bool ForwardVector;
        public bool UpVector;
        public bool CameraUpVector;
        public bool DirectionTowardsCamera;
        public bool DirectionTowardsTarget;
        public GameObject Target;

        public void Update()
        {
            if (ForwardVector) Debug.DrawRay(transform.position, transform.forward, Color.cyan);
            if (UpVector)  Debug.DrawRay(transform.position, transform.up, Color.black);
            if (CameraUpVector) Debug.DrawRay(transform.position, Camera.main.transform.rotation * Vector3.up, Color.red);
            if (DirectionTowardsCamera) Debug.DrawRay(transform.position, transform.position + Camera.main.transform.rotation * Vector3.forward, Color.blue); 
            if (DirectionTowardsTarget && Target != null) Debug.DrawRay(transform.position, transform.position.Direction(Target.transform.position), Color.magenta);
        }
    }
}
