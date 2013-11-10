using Assets.Src.net.kibotu.sandbox.unity.dragnslay.game;
using Assets.Src.net.kibotu.sandbox.unity.dragnslay.model;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.behaviours
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
            if (startTime > 3f && GetComponentsInChildren<Transform>().Length < 3)
            {
                startTime = 0;
                var plane = GameObjectFactory.CreatePlane();
                plane.transform.Translate(transform.position);
                plane.transform.parent = transform;
                Registry.Instance.Planes.Add(plane.GetInstanceID(),plane);
                //Debug.Log("add plane: " + plane.go.GetInstanceID());
            }
        }
    }
}
