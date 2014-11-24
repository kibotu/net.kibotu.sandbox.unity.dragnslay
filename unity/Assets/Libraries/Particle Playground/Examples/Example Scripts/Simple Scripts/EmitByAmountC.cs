using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class EmitByAmountC : MonoBehaviour {

	public int amount = 10;											// The amount of particles you wish to emit
	public Vector3 position;										// The position
	public Vector3 minimumVelocity = new Vector3(-10f,-10f,-10f);	// The minimum random velocity
	public Vector3 maximumVelocity = new Vector3(10f,10f,10f);		// The maximum random velocity
	public Color color = Color.white;								// The color
	public PlaygroundParticlesC particles;							// The particle system reference

	void Start () {

		// Try to get the PlaygroundParticlesC component from this object if set to null
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();
	}

	void Update () {

		// As an example, emit when pressing left mouse-button.
		// This version of Emit () processes random velocities within range set by you. Initial Velocity Shape will apply when available.
		if (Input.GetMouseButtonDown (0))
			particles.Emit (amount, position, minimumVelocity, maximumVelocity, color);
	}
}
