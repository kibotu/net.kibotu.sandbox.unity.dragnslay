using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PlaygroundRotateWithKeys : MonoBehaviour {

	public float speed = 200f;
	Transform thisTransform;

	// Use this for initialization
	void Start () {
		thisTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		thisTransform.RotateAround (Vector3.zero, thisTransform.up, -Input.GetAxis("Horizontal")*speed*Time.deltaTime);
		thisTransform.RotateAround (Vector3.zero, thisTransform.right, Input.GetAxis("Vertical")*speed*Time.deltaTime);
	}
}
