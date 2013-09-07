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

        public Orb(string id, int textureId, GameObject go, TrabantPrototype type, float currentPopulation, int maxPopulation, float spawnPerSec, Life life, PhysicalProperty physicalProperty)
        {
            this.id = id;
            this.textureId = textureId;
            this.go = go;
            this.type = type;
            this.currentPopulation = currentPopulation;
            this.maxPopulation = maxPopulation;
            this.spawnPerSec = spawnPerSec;
            this.life = life;
            this.physicalProperty = physicalProperty;
        }

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

