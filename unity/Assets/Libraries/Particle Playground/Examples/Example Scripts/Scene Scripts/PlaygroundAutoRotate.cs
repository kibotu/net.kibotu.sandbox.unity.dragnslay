using UnityEngine;
using System.Collections;

public class PlaygroundAutoRotate : MonoBehaviour {

	public float rotationSpeed = 10f;
	Transform thisTransform;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		thisTransform.RotateAround (Vector3.zero, Vector3.up, rotationSpeed*Time.deltaTime);
	}
}
