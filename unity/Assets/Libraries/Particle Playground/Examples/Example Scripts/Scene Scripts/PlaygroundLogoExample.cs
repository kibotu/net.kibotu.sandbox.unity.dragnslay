using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class PlaygroundLogoExample : MonoBehaviour {

	public float waitBeforeTurbulence = 3f;					// Seconds to wait before turbulence starts
	public float waitBeforeStopEmission = 0f;				// Seconds to wait before stopping emission
	public float waitWhenDone = 4f;							// Seconds to wait when routine has completed
	public float turbulenceIncreaseSpeed = 2f;				// Speed to increase turbulence with
	public float turbulenceMaxStrength = 4f;				// Max turbulence strength to increase to
	public bool repeat = false;								// Should the effect repeat?
	PlaygroundParticlesC particles;							// Private reference to the particle system
	
	IEnumerator Start () {

		// Get a reference to the particle system
		if (particles==null)
			particles = GetComponent<PlaygroundParticlesC>();

		// Make sure this particle system is reset (upon repeat)
		particles.emit = true;
		particles.turbulenceStrength = 0;

		// Wait before increasing turbulence strength
		yield return new WaitForSeconds(waitBeforeTurbulence);

		// Increase turbulence strength
		while (particles.turbulenceStrength<turbulenceMaxStrength) {
			particles.turbulenceStrength += turbulenceIncreaseSpeed*Time.deltaTime;
			yield return null;
		}

		// Wait before emission stop
		yield return new WaitForSeconds(waitBeforeStopEmission);

		// Stop emission
		particles.emit = false;

		// Wait to continue when sequence is done (could for instance be the lifetime of particles)
		yield return new WaitForSeconds(waitWhenDone);

		// Repeat
		if (repeat)
			StartCoroutine(Start());
		else {
			//If not repeating, add Application.LoadLevel("Your Scene Name") here for instance
		}
	}

}
