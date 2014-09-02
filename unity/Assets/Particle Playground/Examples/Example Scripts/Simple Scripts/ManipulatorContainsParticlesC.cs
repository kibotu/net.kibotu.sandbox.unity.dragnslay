using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class ManipulatorContainsParticlesC : MonoBehaviour {

	public int manipulatorNumber = 0;			// The number of manipulator to get in list
	public PlaygroundParticlesC particles;		// The particle system you wish to address
	public bool localManipulator = false;		// Is this a local manipulator on the particle system?
	ManipulatorObjectC manipulator;				// Cached version of maipulator

	void Start () {

		// Set Cache
		if (localManipulator)
			manipulator = PlaygroundC.GetManipulator(manipulatorNumber, particles);
		else
			manipulator = PlaygroundC.GetManipulator(manipulatorNumber);
			
	}

	void Update () {
		for (int i = 0; i<particles.particleCache.Length; i++)
			if (manipulator.Contains(particles.particleCache[i].position, manipulator.transform.position))
				IsWithin (i);
	}

	void IsWithin (int i) {

		// Do something here with current particle instead of write to console...
		// Most data of each particle can be found in particles.playgroundCache
		Debug.Log("Particle "+i.ToString ()+" is within manipulator");
	}
}
