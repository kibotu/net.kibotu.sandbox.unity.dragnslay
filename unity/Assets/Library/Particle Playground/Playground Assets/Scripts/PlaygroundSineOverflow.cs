using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PlaygroundSineOverflow : MonoBehaviour {

	PlaygroundParticlesC particles;
	float amount = .1f;
	Vector3 initialOverflow;

	// Use this for initialization
	void Start () {
		particles = GetComponent<PlaygroundParticlesC>();
		initialOverflow = particles.overflowOffset;
	}
	
	// Update is called once per frame
	void Update () {
		particles.overflowOffset = initialOverflow+new Vector3(Mathf.Cos (Time.time)*amount, Mathf.Sin (Time.time)*amount, Mathf.Cos (Time.time)*amount);
	}
}
