using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PlaygroundCycleSnapshots : MonoBehaviour {

	PlaygroundParticlesC particles;
	PlaygroundScenes sceneScript;

	void Start () {
		particles = GetComponent<PlaygroundParticlesC>();
		sceneScript = FindObjectOfType<PlaygroundScenes>();
	}
	
	void OnGUI () {
		if (GUI.Button (new Rect(Screen.width-160, 10, 150, 32), particles.snapshots[particles.loadFrom%particles.snapshots.Count].name)) {
			particles.Load (++particles.loadFrom);
			if (sceneScript!=null)
				sceneScript.UpdateParticlesLabel();
		}
	}
}
