using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.scripts
{
    public class SpawnUnits : MonoBehaviour
    {

        public float startTime;

        // Use this for initialization
        void Start ()
        {
            startTime = 0;
        }
	
        // Update is called once per frame
        void Update ()
        {
            startTime += Time.deltaTime;
            if (startTime > 3f && GetComponentsInChildren<Transform>().Length < 2)
            {
                startTime = 0;
                TrabantPrototype plane = OrbFactory.createPlane();
                plane.go.transform.Translate(transform.position);
                plane.go.transform.parent = transform;
                Registry.Instance.Planes.Add(plane.go.GetInstanceID(),plane);
            }
        }
    }
}
