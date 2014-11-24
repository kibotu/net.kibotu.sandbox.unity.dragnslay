using UnityEngine;
using System.Collections;

public class PlaygroundSineRotation : MonoBehaviour {

	Transform thisTransform;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		thisTransform.Rotate(new Vector3(Mathf.Sin (Time.time), Mathf.Cos (Time.time), Mathf.Sin (Time.time)));
	}
}
