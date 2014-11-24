using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class EmitAtMousePosition : MonoBehaviour {

	public PlaygroundParticlesC particles; 	// Assign your particle system in Inspector
	public Vector3 velocity;				// Set velocity in Inspector
	public Color32 color = Color.white; 	// Set color in Inspector

	void Start () {

		// Assume script is assigned to particle system's GameObject if particles is null
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();
	}

	void Update () {

		// Emit on left-click
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000f)) {
				particles.Emit (
					hit.point,
					velocity,
					color
				);
			}
		}
	}
}
