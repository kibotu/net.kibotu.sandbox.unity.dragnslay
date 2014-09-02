using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PauseCalculationByDistanceAngleC : MonoBehaviour {

	public PlaygroundParticlesC particles;			// Assing your particle system to the Inspector
	public Transform target;						// Target to measure distance from
	public float distance = 100f;					// Distance from object to particle system
	public float angle = 90f;						// Angle from object to particle system
	
	void Start () {

		// Try assigning from this GameObject if particles is null
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();
	}
	
	void Update () {
		particles.calculate = (Vector3.Distance(particles.particleSystemTransform.position, target.position)<distance && Vector3.Angle (particles.particleSystemTransform.position-target.position, target.forward)<angle);
	}

}
