using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class ParticleEventListenerC : MonoBehaviour {

	public PlaygroundParticlesC particles;
	PlaygroundEventC playgroundEvent;

	Vector3 gizmoPosition;

	void Start () {

		// Get the first event
		playgroundEvent = PlaygroundC.GetEvent(0, particles);

		// Add listener
		AddListener();
	}

	// Subscribe DoSomething() to the Particle Event
	void AddListener () {
		playgroundEvent.particleEvent += DoSomething;
	}

	// DoSomething will run when a particle triggers the Event
	void DoSomething (PlaygroundEventParticle particle) {
		gizmoPosition = particle.position;
	}

	// Draw the event as a gizmo in Scene View
	void OnDrawGizmos () {
		Gizmos.DrawWireSphere(gizmoPosition, 1f);
	}
}
