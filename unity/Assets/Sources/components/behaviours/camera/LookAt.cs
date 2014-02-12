using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
	// look at 
    public GameObject target;
	public Camera camera;

	void Start() {
		if (camera == null)
			camera = Camera.main;

		if (target == null)
			target = gameObject;
	}
	
	void Update () {
        camera.transform.LookAt(target.transform);
	}
}