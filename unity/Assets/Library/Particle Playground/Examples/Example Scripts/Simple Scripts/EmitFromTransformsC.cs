using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ParticlePlayground;

public class EmitFromTransformsC : MonoBehaviour {

	// Particle system variables
	public PlaygroundParticlesC particles;			// Add your particle system in Inspector
	public int emissionAmount = 1;					// The desired emission amount per emission rate
	public float emissionRate = .1f;				// The desired emission rate 
	public Color32 color = Color.white;				// The desired color (Change Rendering > Color Source to bypass scripted Source color)
	public Vector3 randomPositionMin;				// The minimum random position from transform
	public Vector3 randomPositionMax;				// The maximum random position from transform 
	public Vector3 randomVelocityMin;				// The minimum random velocity
	public Vector3 randomVelocityMax;				// The maximum random velocity

	// Source transform
	public List<Transform> transforms;				// Add your transforms in Inspector or by using transforms.Add(Transform)

	float lastTimeUpdated = 0;
	void Update () {

		if (Time.time>lastTimeUpdated+emissionRate) {

			// Iterate through each transform
			foreach (Transform t in transforms) {
				particles.Emit (
					emissionAmount, 
					t.position+randomPositionMin,
					t.position+randomPositionMax, 
					randomVelocityMin, 
					randomVelocityMax, 
					color
				);
			}

			lastTimeUpdated = Time.time;
		}
	}
}
