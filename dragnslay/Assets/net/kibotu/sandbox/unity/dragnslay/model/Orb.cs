using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.model
{
    public class Orb {
	
        // id
        public string id;

        // grafic
        public int textureId;
        public GameObject go;
	
        // spawning
        public TrabantPrototype type;
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
}

