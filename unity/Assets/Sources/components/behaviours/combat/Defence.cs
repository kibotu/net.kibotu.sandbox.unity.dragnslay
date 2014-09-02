using Assets.Sources.components.data;
using Assets.Sources.model;
using UnityEngine;

namespace Assets.Sources.components.behaviours.combat
{
    public class Defence : MonoBehaviour
    {
        public LifeData LifeData;
        private float _startTime;
        private bool _isExploding;

        public void Start()
        {
            LifeData = GetComponent<LifeData>();
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
