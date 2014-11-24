using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ParticlePlayground;

public class SwitchCachedWorldObjectsC : MonoBehaviour {

	public Transform[] worldObjects;						// The objects in scene you wish to create World Objects from
	public PlaygroundParticlesC particles;					// The Particle Playground system that will change World Object
	List<WorldObject> cachedWorldObjects;					// List of cached World Objects
	
	void Start () {
		
		// Cache the World Objects
		cachedWorldObjects = new List<WorldObject>();
		foreach (Transform wo in worldObjects)
			cachedWorldObjects.Add(PlaygroundC.WorldObject(wo));
		
		// Assign a World Object by list number (example)
		SwitchWorldObject(0);
	}
	
	// Call SwitchWorldObject (int) to assign a new World Object to your particles
	void SwitchWorldObject (int switchTo) {
		particles.worldObject = cachedWorldObjects[switchTo];
	}
}
