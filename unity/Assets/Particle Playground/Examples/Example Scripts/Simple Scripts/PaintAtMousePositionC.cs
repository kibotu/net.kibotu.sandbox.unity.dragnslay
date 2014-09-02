using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PaintAtMousePositionC : MonoBehaviour {

	public PlaygroundParticlesC particles;			// Assign a Particle Playground system through Inspector
	public COLLISIONTYPEC collisionType;			// The type of collision for paint to attach to
	public Color color = Color.white;				// Color to paint with

	// Use this for initialization
	void Start () {

		// Get reference to particle system on GameObject if null
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();
		if (particles!=null) {

			// Make sure particles are set to paint and has chosen collision type
			particles.source = SOURCEC.Paint;
			particles.paint.collisionType = collisionType;

			// Set delta movement to false, otherwise paint will make particles fly on initiation
			particles.calculateDeltaMovement = false;
		} else {
			Debug.Log("Please assign a particle system to this script.", gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 1000f)) {
				PlaygroundC.Paint (particles, hit.point, hit.normal, hit.transform, color);
			}
		}
	}
}
