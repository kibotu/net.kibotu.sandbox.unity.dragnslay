using UnityEngine;

namespace Assets.Sources.components.behaviours.camera
{
    public class LookAt : MonoBehaviour
    {
        // look at 
        public GameObject Target;
        public Camera Camera;

        public void Start()
        {
            if (Camera == null)
                Camera = Camera.main;

            if (Target == null)
                Target = gameObject;
        }
	
        public void Update () {
            Camera.transform.LookAt(Target.transform);
        }
    }
}