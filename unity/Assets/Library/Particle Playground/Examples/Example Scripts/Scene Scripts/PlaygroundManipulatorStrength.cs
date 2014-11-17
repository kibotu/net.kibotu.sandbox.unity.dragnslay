using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PlaygroundManipulatorStrength : MonoBehaviour {

	public float maxStrength = 3f;
	public float adjustmentSpeed = 2f;
	ManipulatorObjectC manipulator;

	// Use this for initialization
	void Start () {
		manipulator = PlaygroundC.GetManipulator (0, GetComponent<PlaygroundParticlesC>());
	}
	
	// Update is called once per frame
	void Update () {
		manipulator.strength = Input.GetMouseButton(0)?
			Mathf.Lerp (manipulator.strength, maxStrength, adjustmentSpeed*Time.deltaTime):
			Mathf.Lerp (manipulator.strength, 0, adjustmentSpeed*Time.deltaTime);
	}
}
