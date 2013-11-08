using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components
{
    public class Orb {
	
        // id
        public int id;

        // grafic
        public int textureId;
        public GameObject go;
	
        // spawning
        public TrabantPrototype type;
        public float currentPopulation;
        public int maxPopulation;
        public float spawnPerSec;
	
        // properties
        public LifeData LifeData;
        public PhysicData PhysicData;

        public Orb(int id, int textureId, GameObject go, TrabantPrototype type, float currentPopulation, int maxPopulation, float spawnPerSec, LifeData LifeData, PhysicData PhysicData)
        {
            this.id = id;
            this.textureId = textureId;
            this.go = go;
            this.type = type;
            this.currentPopulation = currentPopulation;
            this.maxPopulation = maxPopulation;
            this.spawnPerSec = spawnPerSec;
            this.LifeData = LifeData;
            this.PhysicData = PhysicData;
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

