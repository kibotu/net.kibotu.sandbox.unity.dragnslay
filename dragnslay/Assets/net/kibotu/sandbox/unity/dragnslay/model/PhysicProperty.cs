using UnityEngine;
using System.Collections;

public class PhysicProperty {
	
	public float[] position;
	public float[] scalling;
	public float[] rotation;
	
	public int acceleration;
	public int mass;
	public float rotationSpeed;
	public float rotationDistance;
	
	public int speed(int t, int v0) {
		return acceleration * t + v0;
	}
}

