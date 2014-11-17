using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PlaygroundEventListener : MonoBehaviour {

	// Variables for the Event
	public PlaygroundParticlesC particles;		// Assign the particle system in Inspector
	PlaygroundEventC playgroundEvent;			// The reference to the Event

	// Variables for this GameObject
	Transform thisTransform;
	Collider thisCollider;
	Renderer thisRenderer;
	float localAxisRotation;
	float collisionAmplifier = 20f;
	float rotationDamping = 1f;
	bool isActive = true;
	static Material activeMaterial;
	static Material inactiveMaterial;

	void Start () {

		// Get the event from your particle system
		playgroundEvent = PlaygroundC.GetEvent (0, particles);

		// Subscribe to the event
		AddEventListener();

		// Cache components of this GameObject (helps performance on low-end devices)
		thisTransform = transform;
		thisCollider = collider;
		thisRenderer = renderer;

		// Create materials to show if the event listener is active or not
		if (activeMaterial==null) {
			activeMaterial = new Material(Shader.Find ("Diffuse"));
			activeMaterial.color = Color.white;
		}
		if (inactiveMaterial==null) {
			inactiveMaterial = new Material(Shader.Find ("Diffuse"));
			inactiveMaterial.color = Color.black;
		}

		thisRenderer.sharedMaterial = activeMaterial;
	}
	
	// Run ParticleEvent each time a particle sends an Event
	void ParticleEvent (PlaygroundEventParticle particle) {

		// If the particle's collider is this then change the localAxisRotation based on particle's size and velocity
		if (particle.collisionCollider == thisCollider)
			localAxisRotation += particle.size*particle.velocity.magnitude*collisionAmplifier;
	}

	// Subscribe the ParticleEvent function to the event delegate of particleEvent
	void AddEventListener () {
		playgroundEvent.particleEvent += ParticleEvent;
	}

	// Unsubscribe the ParticleEvent function of the event delegate of particleEvent
	void RemoveEventListener () {
		playgroundEvent.particleEvent -= ParticleEvent;
	}

	void Update () {

		// Rotate this transform with localAxisRotation
		thisTransform.RotateAround (thisTransform.position, thisTransform.forward, -localAxisRotation*PlaygroundC.globalDeltaTime);

		// Damp the localAxisRotation
		localAxisRotation = Mathf.Lerp (localAxisRotation, 0, rotationDamping*PlaygroundC.globalDeltaTime);
	}

	void OnMouseUp () {
		isActive = !isActive;
		thisRenderer.sharedMaterial = isActive?activeMaterial:inactiveMaterial;
		if (isActive) AddEventListener(); else RemoveEventListener();
	}
}
