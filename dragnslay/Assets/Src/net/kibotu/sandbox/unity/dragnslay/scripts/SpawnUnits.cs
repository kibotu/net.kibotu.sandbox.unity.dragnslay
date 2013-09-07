using Assets.net.kibotu.sandbox.unity.dragnslay.model;
using UnityEngine;

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
        if (startTime > 3f)
        {
            startTime = 0;
            TrabantPrototype plane = OrbFactory.createPlane();
            plane.go.transform.Translate(transform.position);
            plane.go.transform.parent = transform;
        }
	}
}
