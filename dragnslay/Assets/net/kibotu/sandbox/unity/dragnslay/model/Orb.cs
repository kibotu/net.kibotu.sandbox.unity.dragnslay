using UnityEngine;
using System.Collections;

public class Orb {
	
	// grafic
	public int textureId;
	public float[] position;
	public float[] scalling;
	public float[] rotation;
	
	// spawning
	public ShipPrototype type;
	public float currentPopulation;
	public int maxPopulation;
	public float spawnPerSec;
	
	// properties
	public Life life;
	public PhysicalProperty physicalProperty;
	
	public void spawn(float dt) {
	}
	
	public void moveUnitsTo(int amount, Orb destination) {
	}
	
	public void defend() {
	}
	
	public void attack() {
	}
}

