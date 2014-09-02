using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class KillParticleWithinEventRangeC : MonoBehaviour {

	public float killRange = 1f;
	public PlaygroundParticlesC particles;		// Assign the particle system in Inspector
	PlaygroundEventC playgroundEvent;			// The reference to the Event

	Transform thisTransform;
	Vector3 thisPosition;

	// Use this for initialization
	void Start () {

		// Events run on a second thread, only use thread-safe methods within the Event Delegate (no GetTransform)
		thisTransform = transform;

		// Get the event from your particle system
		playgroundEvent = PlaygroundC.GetEvent (0, particles);
		
		// Subscribe to the event
		AddEventListener();
	}

	void Update () {

		// Events run on a second thread, only use thread-safe methods within the Event Delegate (no GetPosition) 
		thisPosition = thisTransform.position;
	}

	// Run ParticleEvent each time a particle sends an Event
	void ParticleEvent (PlaygroundEventParticle particle) {
		if (Vector3.Distance (particle.position, thisPosition) <= killRange)
			particles.Kill (particle.particleId);
	}
	
	// Subscribe the ParticleEvent function to the event delegate of particleEvent
	void AddEventListener () {
		playgroundEvent.particleEvent += ParticleEvent;
	}
	
	// Unsubscribe the ParticleEvent function of the event delegate of particleEvent
	void RemoveEventListener () {
		playgroundEvent.particleEvent -= ParticleEvent;
	}
}
