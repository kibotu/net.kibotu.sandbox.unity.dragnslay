using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.model
{
    public class TrabantPrototype {
	
        // graic
        public int model; 		// mesh id/type
        public int textureId; 
        public GameObject go;
	
        public Life life;
        public PhysicalProperty physicalProperty;

        public TrabantPrototype(int model, int textureId, GameObject go, Life life, PhysicalProperty physicalProperty)
        {
            this.model = model;
            this.textureId = textureId;
            this.go = go;
            this.life = life;
            this.physicalProperty = physicalProperty;
        }

        public void moveTo(Orb dest) {
        }
	
        public void defend() {
        }
	
        public void attack() {
        }
    }
}

