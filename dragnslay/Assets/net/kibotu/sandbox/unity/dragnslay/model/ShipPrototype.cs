using UnityEngine;
using System.Collections;

public abstract class ShipPrototype {
	
	public int model; 			// mesh id/type
	public int textureId; 		
	
	public int hp;
	public int armor;
	public int shield;
	public int acceleration;
	public int mass;
	public float hp_regen; 		// hp 				/ sec
	public float armor_repair;  // armor_repair 	/ sec
	public float shield_regen;  // shield_regen 	/ sec
	
	public int speed(int t, int v0) {
		return a * t + v0;
	}
}

