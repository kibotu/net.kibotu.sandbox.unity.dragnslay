using Assets.Src.net.kibotu.sandbox.unity.dragnslay.components.data;
using UnityEngine;

namespace Assets.Src.net.kibotu.sandbox.unity.dragnslay.components
{
    public class TrabantPrototype {
	
        // graic
        public int model; 		// mesh id/type
        public int textureId; 
        public GameObject go;
	
        public LifeData LifeData;
        public PhysicData PhysicData;

        public TrabantPrototype(int model, int textureId, GameObject go, LifeData LifeData, PhysicData PhysicData)
        {
            this.model = model;
            this.textureId = textureId;
            this.go = go;
            this.LifeData = LifeData;
            this.PhysicData = PhysicData;
        }

        public void moveTo(Orb dest) {
        }
	
        public void defend() {
        }
	
        public void attack() {
        }
    }
}

