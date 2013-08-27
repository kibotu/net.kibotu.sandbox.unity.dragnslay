using UnityEngine;
using System.Collections;

public abstract class TrabantPrototype {
	
	// graic
	public int model; 		// mesh id/type
	public int textureId; 
	
	public Life life;
	public PhysicalProperty physicalProperty;
	
	public void moveTo(Orb dest) {
	}
	
	public void defend() {
	}
	
	public void attack() {
	}
}

