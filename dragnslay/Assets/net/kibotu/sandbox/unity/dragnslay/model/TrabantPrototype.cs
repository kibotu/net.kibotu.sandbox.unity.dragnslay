using UnityEngine;

namespace Assets.net.kibotu.sandbox.unity.dragnslay.model
{
    public class TrabantPrototype {
	
        // graic
        public int model; 		// mesh id/type
        public int textureId; 
        public GameObject go;
	
        public Life life;
        public PhysicalProperty physicalProperty;
	
        public void moveTo(Orb dest) {
        }
	
        public void defend() {
        }
	
        public void attack() {
        }
    }
}

