using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ParticlePlayground;

/*
 * Example of particle system pooling for PlaygroundParticlesC
 * Attach this script to a GameObject in your scene, set the particle system prefab you wish to pool and the quantity.
 * Note: You may want a non-looping system set to Disable On Done.
 */

public class SwitchCachedParticleSystemC : MonoBehaviour {
	
	public GameObject particleSystemPrefab;							// The prefab to instantiate
	public int quantity = 10;										// The number of pooled particle systems
	int currentEnabled = 0;											// The current enabled particle system
	List <PlaygroundParticlesC> cachedParticles;					// The pooled particle systems
	
	void Start () {
		
		// Cache the particle systems
		cachedParticles = new List<PlaygroundParticlesC>();
		for (int i = 0; i<quantity; i++) {
			GameObject go = (GameObject)Instantiate ((Object)particleSystemPrefab);
			cachedParticles.Add (go.GetComponent<PlaygroundParticlesC>());
			cachedParticles[i].particleSystemGameObject.SetActive(false);
		}
	}
	
	void Update () {
		
		// Enable a particle system at mouse position on click (example)
		RaycastHit hit;
		if (Input.GetMouseButtonDown (0) && Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			EnableParticleSystem (hit.point);
	}
	
	// Enable next particle system
	void EnableParticleSystem (Vector3 position) {
		cachedParticles[currentEnabled].particleSystemTransform.position = position;
		cachedParticles[currentEnabled].Emit (true);
		currentEnabled++;
		currentEnabled = currentEnabled%cachedParticles.Count;
	}
}
