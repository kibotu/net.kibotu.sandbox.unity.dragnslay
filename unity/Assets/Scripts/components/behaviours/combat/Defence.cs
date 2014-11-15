using Assets.Sources.components.data;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.components.behaviours.combat
{
    public class Defence : MonoBehaviour
    {
        public LifeData LifeData;
        public float _startTime;
        private bool _isExploding;
		private GameObject shield;
		public float ShieldDistance = 0.7f;

        public void Start()
        {
            LifeData = GetComponent<LifeData>();
        }

		public void Defend(RocketMove rocket) 
		{
			// reduce hitpoints
			Defend(rocket.AttackDamage);
			
			// spawn shield
		    if (shield == null)
		    {
		        shield = Prefabs.Instance.GetNewShield();
                shield.renderer.material.SetColor("_Tint", Color.red);
		    }
			else 
				shield.GetComponent<FadeOutAndDestroy>().Reset();

			shield.transform.forward = -rocket.dir;
			shield.transform.position = transform.position - rocket.dir * ShieldDistance;
			shield.transform.parent = transform;
		}

        public void Defend(float damage)
        {
            LifeData.CurrentHp -= damage;
            if (LifeData.CurrentHp > 0 || _isExploding) return;

            Destroy(gameObject);
            var explosion = Prefabs.Instance.GetNewExplosion();
            explosion.transform.position = transform.position;
//            explosion.GetComponent<DetonatorShockwave>().color = GetComponent<ShipData>().PlayerData.color;
            
            Registry.Ships.Remove(GetComponent<ShipData>().uid);

//            Debug.Log(GetComponent<ShipData>().uid + " has been destroyed!");
//            _isExploding = true;
        }
    }
}
